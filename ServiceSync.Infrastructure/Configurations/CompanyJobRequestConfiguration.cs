using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations;

public class CompanyJobRequestConfiguration : IEntityTypeConfiguration<CompanyJobRequest>
{
    public void Configure(EntityTypeBuilder<CompanyJobRequest> builder)
    {
        builder
            .HasKey(cc => new { cc.CompanyId, cc.JobRequestId });
        builder
           .HasOne(cjr => cjr.Company)
           .WithMany(c => c.JobRequests)
           .HasForeignKey(cc => cc.CompanyId);
        builder
           .HasOne(cjr => cjr.JobRequest)
           .WithMany(c => c.Companies)
           .HasForeignKey(cc => cc.JobRequestId);
    }
}