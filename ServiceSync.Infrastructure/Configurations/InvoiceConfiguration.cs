using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CreatorId).IsRequired();
        builder.Property(c => c.JobRequestId).IsRequired();
        builder.Property(c => c.EstimateReady);
        builder.Property(c => c.EstimateApproved);
        builder.Property(c => c.EstimateApprovedIP);
        builder.Property(c => c.PaymentDueDate);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt).IsRequired();
        builder
            .HasMany(i => i.InvoiceLineItems)
            .WithOne(ili => ili.Invoice)
            .HasForeignKey(ili => ili.InvoiceId);
    }
}