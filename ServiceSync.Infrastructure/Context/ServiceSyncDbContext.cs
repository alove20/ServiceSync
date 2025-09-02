using Microsoft.EntityFrameworkCore;
using ServiceSync.Core.Models;
using System.Reflection;

namespace ServiceSync.Infrastructure.Context;

public class ServiceSyncDbContext(DbContextOptions<ServiceSyncDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    public DbSet<CompanyClient> CompanyClients { get; set; }
    public DbSet<CompanyJobRequest> CompanyJobRequests { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<JobRequest> JobRequests { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<LineItem> LineItems { get; set; }
    public DbSet<LineItemType> LineItemTypes { get; set; }
    public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
    public DbSet<JobRequestInvoice> JobRequestInvoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}