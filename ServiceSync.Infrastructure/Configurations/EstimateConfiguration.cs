using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;
using ServiceSync.Core.Enums;

namespace ServiceSync.Infrastructure.Configurations
{
    public class EstimateConfiguration : IEntityTypeConfiguration<Estimate>
    {
        public void Configure(EntityTypeBuilder<Estimate> builder)
        {
            builder.ToTable("Estimates");

            builder.HasKey(e => e.Id);

            // Configure the relationship to the JobRequest
            builder.HasOne(e => e.JobRequest)
                .WithMany()
                .HasForeignKey(e => e.JobRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship to the Contact who created the estimate
            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the Status property to be stored as an integer
            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(EstimateStatus.Draft);

            builder.HasMany(e => e.EstimateLineItems)
                .WithOne(eli => eli.Estimate)
                .HasForeignKey(eli => eli.EstimateId);
        }
    }
}

