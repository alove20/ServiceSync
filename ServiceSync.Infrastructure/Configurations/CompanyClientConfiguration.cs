using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations;

public class CompanyClientConfiguration : IEntityTypeConfiguration<CompanyClient>
{
    public void Configure(EntityTypeBuilder<CompanyClient> builder)
    {
        builder
            .HasKey(cc => new { cc.CompanyId, cc.ClientId });
        builder
           .HasOne(cc => cc.Company)
           .WithMany(c => c.Clients)
           .HasForeignKey(cc => cc.CompanyId);
        builder
           .HasOne(cc => cc.Client)
           .WithMany(c => c.ClientCompanies)
           .HasForeignKey(cc => cc.ClientId);
    }
}