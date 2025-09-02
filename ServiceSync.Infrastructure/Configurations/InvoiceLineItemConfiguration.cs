using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;
using System.Reflection.Emit;

namespace ServiceSync.Infrastructure.Configurations;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
    {
        builder.HasKey(ili => new { ili.InvoiceId, ili.LineItemId });
        builder.Property(c => c.Quantity).IsRequired();
        builder.Property(c => c.PriceOverride);
        builder.Property(c => c.Notes).IsRequired();
        builder
            .HasOne(ili => ili.LineItem)
            .WithOne(li => li.InvoiceLineItem)
            .HasForeignKey<InvoiceLineItem>(ili => ili.LineItemId);
    }
}