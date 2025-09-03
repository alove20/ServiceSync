using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceSync.Infrastructure.Context;
using ServiceSync.WebApi.GraphQL;
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
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

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
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

            builder.Services.AddAuthorization();

            var allowedSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowedSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                            // .WithOrigins("http://localhost:5173", "null") // To allow requests from file:// URLs
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });
            builder.Services.AddPooledDbContextFactory<ServiceSyncDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            // 2. Add and configure the GraphQL server
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
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.MapOpenApi();

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
