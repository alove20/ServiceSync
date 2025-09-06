using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class JobRequestConfiguration : IEntityTypeConfiguration<JobRequest>
    {
        public void Configure(EntityTypeBuilder<JobRequest> builder)
        {
            builder.ToTable("JobRequests");

            builder.HasKey(jr => jr.Id);

            builder.Property(jr => jr.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(jr => jr.Description)
                .IsRequired();

            // Configures the one-to-many relationship where a JobRequest has one Client (Contact),
            // and a Contact can have many created JobRequests.
            builder.HasOne(jr => jr.Client)
                .WithMany(c => c.CreatedJobRequests)
                .HasForeignKey(jr => jr.ClientId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a client if they have jobs

            // Configures the one-to-many relationship with the CompanyJobRequest junction table
            builder.HasMany(jr => jr.Companies)
                .WithOne(cjr => cjr.JobRequest)
                .HasForeignKey(cjr => cjr.JobRequestId);

            // Configures the one-to-many relationship with the ResourceJobRequest junction table
            builder.HasMany(jr => jr.Resources)
                .WithOne(rjr => rjr.JobRequest)
                .HasForeignKey(rjr => rjr.JobRequestId);

            // Configures the one-to-many relationship with the JobRequestEstimate junction table
            builder.HasMany(jr => jr.Estimates)
                .WithOne(jre => jre.JobRequest)
                .HasForeignKey(jre => jre.JobRequestId);
        }
    }
}
