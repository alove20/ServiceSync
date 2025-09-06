using Microsoft.EntityFrameworkCore;
using ServiceSync.Core.Models;
using System.Reflection;

namespace ServiceSync.Infrastructure.Context;

public class ServiceSyncDbContext(DbContextOptions<ServiceSyncDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyResource> CompanyResources { get; set; }
    public DbSet<CompanyClient> CompanyClients { get; set; }
    public DbSet<CompanyJobRequest> CompanyJobRequests { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<JobRequest> JobRequests { get; set; }
    public DbSet<Estimate> Estimates { get; set; }
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<EstimateTemplate> EstimateTemplates { get; set; }
    public DbSet<EstimateLineItem> EstimateLineItems { get; set; }
    public DbSet<JobRequestEstimate> JobRequestEstimates { get; set; }
    public DbSet<EstimateTemplateItem> EstimateTemplateItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
