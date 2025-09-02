using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations;

public class JobRequestConfiguration : IEntityTypeConfiguration<JobRequest>
{
    public void Configure(EntityTypeBuilder<JobRequest> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.ClientId).IsRequired();
        builder.Property(c => c.Title).HasMaxLength(50);
        builder.Property(c => c.Description).HasMaxLength(255);
        builder.Property(c => c.OpenedAt);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt).IsRequired();
    }
}