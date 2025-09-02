using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(255);
        builder.Property(c => c.AddressLine1).HasMaxLength(255);
        builder.Property(c => c.AddressLine2).HasMaxLength(255);
        builder.Property(c => c.City).HasMaxLength(100);
        builder.Property(c => c.State).HasMaxLength(50);
        builder.Property(c => c.ZipCode).HasMaxLength(20);
        builder.Property(c => c.PhoneNumber).HasMaxLength(20);
        builder.Property(c => c.LogoUrl);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt).IsRequired();
    }
}