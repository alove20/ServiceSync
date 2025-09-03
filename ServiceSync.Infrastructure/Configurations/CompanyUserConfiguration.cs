using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations;

public class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
{
    public void Configure(EntityTypeBuilder<CompanyUser> builder)
    {
        builder
            .HasKey(cu => new { cu.CompanyId, cu.UserId });
        builder
           .HasOne(cu => cu.Company)
           .WithMany(c => c.Users)
           .HasForeignKey(cu => cu.CompanyId);
        builder
            .HasOne(cu => cu.User)
            .WithMany(c => c.UserCompanies)
            .HasForeignKey(cu => cu.UserId);
    }
}