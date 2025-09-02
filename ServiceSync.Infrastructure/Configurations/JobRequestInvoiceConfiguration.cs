using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations;

public class JobRequestInvoiceConfiguration : IEntityTypeConfiguration<JobRequestInvoice>
{
    public void Configure(EntityTypeBuilder<JobRequestInvoice> builder)
    {
        builder
            .HasKey(jri => new { jri.JobRequestId, jri.InvoiceId });
        builder
            .HasOne(jri => jri.JobRequest)
            .WithMany(jr => jr.Invoices)
            .HasForeignKey(jri => jri.JobRequestId);
        builder
            .HasOne(jri => jri.Invoice)
            .WithMany(jr => jr.JobRequests)
            .HasForeignKey(jri => jri.InvoiceId);
    }
}