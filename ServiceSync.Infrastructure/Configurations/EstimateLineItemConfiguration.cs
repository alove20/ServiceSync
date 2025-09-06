using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class EstimateLineItemConfiguration : IEntityTypeConfiguration<EstimateLineItem>
    {
        public void Configure(EntityTypeBuilder<EstimateLineItem> builder)
        {
            builder.ToTable("EstimateLineItems");

            // Configure the composite primary key
            builder.HasKey(eli => new { eli.EstimateId, eli.CatalogItemId });

            // Configure the relationship to the Estimate
            builder.HasOne(eli => eli.Estimate)
                .WithMany(e => e.EstimateLineItems)
                .HasForeignKey(eli => eli.EstimateId);

            // Configure the relationship to the CatalogItem
            builder.HasOne(eli => eli.CatalogItem)
                .WithMany() // A CatalogItem can be in many estimate line items
                .HasForeignKey(eli => eli.CatalogItemId);

            builder.Property(eli => eli.UnitPrice)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(eli => eli.PriceOverride)
                .HasColumnType("money");
        }
    }
}
