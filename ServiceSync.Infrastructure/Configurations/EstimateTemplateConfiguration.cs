using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class EstimateTemplateConfiguration : IEntityTypeConfiguration<EstimateTemplate>
    {
        public void Configure(EntityTypeBuilder<EstimateTemplate> builder)
        {
            builder.ToTable("EstimateTemplates");

            builder.HasKey(et => et.Id);

            builder.Property(et => et.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure the relationship to Company
            // An EstimateTemplate belongs to one Company
            builder.HasOne(et => et.Company)
                .WithMany() // A Company can have many templates
                .HasForeignKey(et => et.CompanyId)
                .IsRequired();

            // Configure the one-to-many relationship with the junction table
            builder.HasMany(et => et.TemplateItems)
                .WithOne(eti => eti.EstimateTemplate)
                .HasForeignKey(eti => eti.EstimateTemplateId);
        }
    }
}
