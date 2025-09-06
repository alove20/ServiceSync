using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class JobRequestEstimateConfiguration : IEntityTypeConfiguration<JobRequestEstimate>
    {
        public void Configure(EntityTypeBuilder<JobRequestEstimate> builder)
        {
            builder.ToTable("JobRequestEstimates");

            // Configure the composite primary key
            builder.HasKey(jre => new { jre.JobRequestId, jre.EstimateId });

            // Configure the relationship to the JobRequest
            builder.HasOne(jre => jre.JobRequest)
                .WithMany(jr => jr.Estimates)
                .HasForeignKey(jre => jre.JobRequestId);

            // Configure the relationship to the Estimate
            builder.HasOne(jre => jre.Estimate)
                .WithMany() // An estimate is for one job, so no navigation property here
                .HasForeignKey(jre => jre.EstimateId);
        }
    }
}