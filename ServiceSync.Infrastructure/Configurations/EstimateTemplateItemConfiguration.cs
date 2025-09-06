using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServiceSync.Infrastructure.Configurations
{
    public class EstimateTemplateItemConfiguration : IEntityTypeConfiguration<EstimateTemplateItem>
    {
        public void Configure(EntityTypeBuilder<EstimateTemplateItem> builder)
        {
            builder.ToTable("EstimateTemplateItems");

            // Configure the composite primary key
            builder.HasKey(eti => new { eti.EstimateTemplateId, eti.CatalogItemId });

            // Configure the relationship to the EstimateTemplate
            builder.HasOne(eti => eti.EstimateTemplate)
                .WithMany(et => et.TemplateItems)
                .HasForeignKey(eti => eti.EstimateTemplateId);

            // Configure the relationship to the CatalogItem
            builder.HasOne(eti => eti.CatalogItem)
                .WithMany() // A CatalogItem can be in many templates
                .HasForeignKey(eti => eti.CatalogItemId);
        }
    }
}
