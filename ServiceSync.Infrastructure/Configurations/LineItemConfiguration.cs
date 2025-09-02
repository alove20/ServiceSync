using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;
using System.Reflection.Emit;

namespace ServiceSync.Infrastructure.Configurations;

public class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Price).IsRequired();
        builder
            .HasOne(li => li.LineItemType)
            .WithMany(lit => lit.LineItems)
            .HasForeignKey(li => li.LineItemTypeId);
    }
}