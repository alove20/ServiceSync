using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(c => c.Role).IsRequired().HasMaxLength(50);
        builder.Property(c => c.IsEmailVerified).IsRequired();
        builder.Property(c => c.VerificationToken).HasMaxLength(255);
        builder.Property(c => c.VerificationTokenExpiresAt);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt).IsRequired();
        builder.HasOne(u => u.Contact)
            .WithOne(c => c.User)
            .HasForeignKey<Contact>(c => c.Id);
        builder
            .HasOne(u => u.Contact)
            .WithOne(c => c.User)
            .HasForeignKey<User>(u => u.Id)
            .HasPrincipalKey<Contact>(c => c.Id);
    }
}