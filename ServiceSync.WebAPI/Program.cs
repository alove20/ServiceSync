using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceSync.Infrastructure.Context;
using ServiceSync.WebApi.GraphQL;
using ServiceSync.WebApi.Services;
using ServiceSync.WebApi.Settings;
using ServiceSync.WebApi.GraphQL.Types; // 1. Import the new Types namespace
using System.Text;

namespace ServiceSync.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(jwtSettingsSection);
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>()!;

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });
            builder.Services.AddAuthorization();

            builder.Services.AddPooledDbContextFactory<ServiceSyncDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            builder.Services.AddScoped<IAuthService, AuthService>();

            // --- START OF DEFINITIVE FIX ---
            builder.Services
               .AddGraphQLServer()
               .RegisterDbContextFactory<ServiceSyncDbContext>()
               .AddQueryType<Query>()
               .AddMutationType<Mutation>()
               .AddType<RoleType>() // 2. Use our explicit RoleType configuration instead of the generic one.
               .AddAuthorization()
               .AddProjections()
               .AddFiltering()
               .AddSorting();
            // --- END OF DEFINITIVE FIX ---

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseCors("_myAllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapGraphQL();
            app.Run();
        }
    }
}

