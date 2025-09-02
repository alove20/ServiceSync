using Microsoft.EntityFrameworkCore;
using ServiceSync.Infrastructure.Context;
using ServiceSync.WebAPI.GraphQL;

namespace ServiceSync.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddPooledDbContextFactory<ServiceSyncDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            builder.Services
              .AddGraphQLServer()
              .RegisterDbContextFactory<ServiceSyncDbContext>()
              .AddQueryType<Query>()
              .AddMutationType<Mutation>()
              .AddProjections()
              .AddFiltering()
              .AddSorting();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.MapOpenApi();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapGraphQL();
            app.MapControllers();
            app.Run();
        }
    }
}
