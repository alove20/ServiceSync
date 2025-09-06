using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class CompanyJobRequestConfiguration : IEntityTypeConfiguration<CompanyJobRequest>
    {
        public void Configure(EntityTypeBuilder<CompanyJobRequest> builder)
        {
            builder.ToTable("CompanyJobRequests");

            // Configure the composite primary key
            builder.HasKey(cjr => new { cjr.CompanyId, cjr.JobRequestId });

            // Configure the relationship to the Company
            builder.HasOne(cjr => cjr.Company)
                .WithMany(c => c.JobRequests)
                .HasForeignKey(cjr => cjr.CompanyId);

            // Configure the relationship to the JobRequest
            builder.HasOne(cjr => cjr.JobRequest)
                .WithMany(jr => jr.Companies)
                .HasForeignKey(cjr => cjr.JobRequestId);
        }
    }
}
