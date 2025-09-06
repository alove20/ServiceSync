using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;
using ServiceSync.Core.Enums;

namespace ServiceSync.Infrastructure.Configurations
{
    public class CompanyResourceConfiguration : IEntityTypeConfiguration<CompanyResource>
    {
        public void Configure(EntityTypeBuilder<CompanyResource> builder)
        {
            builder.ToTable("CompanyResources");

            // Configure the composite primary key
            builder.HasKey(cr => new { cr.CompanyId, cr.ResourceId });

            // Configure the relationship to the Company
            builder.HasOne(cr => cr.Company)
                .WithMany(c => c.Resources)
                .HasForeignKey(cr => cr.CompanyId);

            // Configure the relationship to the Contact (Resource)
            builder.HasOne(cr => cr.Resource)
                .WithMany(c => c.ResourceCompanies)
                .HasForeignKey(cr => cr.ResourceId);

            // Configure the Role property
            builder.Property(cr => cr.Role)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(Role.User);
        }
    }
}
