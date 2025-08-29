using Microsoft.EntityFrameworkCore;
using ServiceSync.Data.Models;

namespace ServiceSync.Data.Context;

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
    public DbSet<JobRequestInvoice> JobRequestInvoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompanyUser>()
           .HasKey(cu => new { cu.CompanyId, cu.ContactId });
        modelBuilder.Entity<CompanyUser>()
           .HasOne(cu => cu.Company)
           .WithMany(c => c.Users)
           .HasForeignKey(cu => cu.CompanyId);
        modelBuilder.Entity<CompanyUser>()
           .HasOne(cu => cu.User)
           .WithMany(c => c.Companies)
           .HasForeignKey(cu => cu.ContactId);

        modelBuilder.Entity<CompanyClient>()
           .HasKey(cc => new { cc.CompanyId, cc.ContactId });
        modelBuilder.Entity<CompanyClient>()
           .HasOne(cc => cc.Company)
           .WithMany(c => c.Clients)
           .HasForeignKey(cc => cc.CompanyId);

        modelBuilder.Entity<CompanyJobRequest>()
           .HasKey(cc => new { cc.CompanyId, cc.JobRequestId });
        modelBuilder.Entity<CompanyJobRequest>()
           .HasOne(cjr => cjr.Company)
           .WithMany(c => c.JobRequests)
           .HasForeignKey(cc => cc.CompanyId);

        modelBuilder.Entity<JobRequestInvoice>()
            .HasKey(cc => new { cc.JobRequestId, cc.InvoiceId });
        modelBuilder.Entity<JobRequestInvoice>()
            .HasOne(jri => jri.JobRequest)
            .WithMany(jr => jr.Invoices)
            .HasForeignKey(jri => jri.JobRequestId);

        modelBuilder.Entity<UserJobRequest>()
            .HasKey(ujr => new { ujr.UserId, ujr.JobRequestId });
        modelBuilder.Entity<UserJobRequest>()
            .HasOne(ujr => ujr.User)
            .WithMany(c => c.JobRequests)
            .HasForeignKey(ujr => ujr.UserId);
        modelBuilder.Entity<UserJobRequest>()
            .HasOne(ujr => ujr.JobRequest)
            .WithMany(jr => jr.Users)
            .HasForeignKey(ujr => ujr.JobRequestId);
    }
}