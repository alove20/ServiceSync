using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceSync.Infrastructure.Context;
using ServiceSync.WebApi.GraphQL;
using ServiceSync.WebApi.Services;
using ServiceSync.WebApi.Settings;
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

            // --- Authentication & Authorization Setup ---
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

            // --- CORS Policy Setup ---
            var allowedSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowedSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // --- Service Registrations ---
            builder.Services.AddPooledDbContextFactory<ServiceSyncDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            // *** THIS IS THE NEW LINE ***
            // Register IHttpContextAccessor to allow access to HttpContext from other services.
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IAuthService, AuthService>();

            // --- GraphQL Server Setup ---
            builder.Services
               .AddGraphQLServer()
               .RegisterDbContextFactory<ServiceSyncDbContext>()
               .AddQueryType<Query>()
               .AddMutationType<Mutation>()
               .AddAuthorization()
               .AddProjections()
               .AddFiltering()
               .AddSorting();

            builder.Services.AddControllers();

            var app = builder.Build();

            // --- HTTP Request Pipeline ---
            app.UseHttpsRedirection();

            app.UseCors(allowedSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGraphQL();
            app.MapControllers();
            app.Run();
        }
    }
}

