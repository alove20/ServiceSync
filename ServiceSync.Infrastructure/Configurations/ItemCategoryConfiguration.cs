using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
    {
        public void Configure(EntityTypeBuilder<ItemCategory> builder)
        {
            builder.ToTable("ItemCategories");

            builder.HasKey(ic => ic.Id);

            builder.Property(ic => ic.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure the relationship to Company
            // An ItemCategory belongs to one Company
            builder.HasOne(ic => ic.Company)
                .WithMany() // A company can have many categories
                .HasForeignKey(ic => ic.CompanyId)
                .IsRequired();

            // Configure the one-to-many relationship with CatalogItem
            builder.HasMany(ic => ic.CatalogItems)
                .WithOne(ci => ci.ItemCategory)
                .HasForeignKey(ci => ci.ItemCategoryId);
        }
    }
}
