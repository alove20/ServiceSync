using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Email)
                .HasMaxLength(255);

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(50);

            // Defines the one-to-many relationship between Company and CompanyResource
            builder.HasMany(c => c.Resources)
                .WithOne(cr => cr.Company)
                .HasForeignKey(cr => cr.CompanyId);

            // Defines the one-to-many relationship between Company and CompanyClient
            builder.HasMany(c => c.Clients)
                .WithOne(cc => cc.Company)
                .HasForeignKey(cc => cc.CompanyId);

            // Defines the one-to-many relationship between Company and CompanyJobRequest
            builder.HasMany(c => c.JobRequests)
                .WithOne(cjr => cjr.Company)
                .HasForeignKey(cjr => cjr.CompanyId);
        }
    }
}
