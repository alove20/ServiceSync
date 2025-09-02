using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;
using System.Reflection.Emit;

namespace ServiceSync.Infrastructure.Configurations;

public class UserJobRequestConfiguration : IEntityTypeConfiguration<UserJobRequest>
{
    public void Configure(EntityTypeBuilder<UserJobRequest> builder)
    {
        builder
            .HasKey(ujr => new { ujr.UserId, ujr.JobRequestId });
        builder
            .HasOne(ujr => ujr.User)
            .WithMany(c => c.JobRequests)
            .HasForeignKey(ujr => ujr.UserId);
        builder
            .HasOne(ujr => ujr.JobRequest)
            .WithMany(jr => jr.Users)
            .HasForeignKey(ujr => ujr.JobRequestId);
    }
}