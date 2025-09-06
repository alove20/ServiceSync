using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class CompanyClientConfiguration : IEntityTypeConfiguration<CompanyClient>
    {
        public void Configure(EntityTypeBuilder<CompanyClient> builder)
        {
            builder.ToTable("CompanyClients");

            // Configure the composite primary key
            builder.HasKey(cc => new { cc.CompanyId, cc.ClientId });

            // Configure the relationship to the Company
            builder.HasOne(cc => cc.Company)
                .WithMany(c => c.Clients)
                .HasForeignKey(cc => cc.CompanyId);

            // Configure the relationship to the Contact (Client)
            builder.HasOne(cc => cc.Client)
                .WithMany(c => c.ClientCompanies)
                .HasForeignKey(cc => cc.ClientId);
        }
    }
}
