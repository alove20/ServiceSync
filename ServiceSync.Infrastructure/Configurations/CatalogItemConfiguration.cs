using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;
using ServiceSync.Core.Enums;

namespace ServiceSync.Infrastructure.Configurations
{
    public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("CatalogItems");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ci => ci.Price)
                .IsRequired()
                .HasColumnType("money");

            // Configure the relationship to ItemCategory
            // A CatalogItem has one optional ItemCategory
            builder.HasOne(ci => ci.ItemCategory)
                .WithMany(ic => ic.CatalogItems)
                .HasForeignKey(ci => ci.ItemCategoryId)
                .IsRequired(false); // An item does not have to belong to a category

            // Configure the UnitType enum
            builder.Property(ci => ci.UnitType)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(UnitType.Fixed);
        }
    }
}
