using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class ResourceJobRequestConfiguration : IEntityTypeConfiguration<ResourceJobRequest>
    {
        public void Configure(EntityTypeBuilder<ResourceJobRequest> builder)
        {
            builder.ToTable("ResourceJobRequests");

            // Configure the composite primary key
            builder.HasKey(rjr => new { rjr.JobRequestId, rjr.ResourceId });

            // Configure the relationship to the JobRequest
            builder.HasOne(rjr => rjr.JobRequest)
                .WithMany(jr => jr.Resources)
                .HasForeignKey(rjr => rjr.JobRequestId);

            // Configure the relationship to the Contact (Resource)
            builder.HasOne(rjr => rjr.Resource)
                .WithMany(c => c.JobAssignments)
                .HasForeignKey(rjr => rjr.ResourceId);
        }
    }
}
