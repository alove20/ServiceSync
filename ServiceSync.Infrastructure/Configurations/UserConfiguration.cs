using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // The primary key for a User is its Id.
            builder.HasKey(u => u.Id);

            // This configures the one-to-one relationship.
            // A User has one Contact, and a Contact has one User.
            // The foreign key is on the User table, and it's the User's own Id column.
            builder.HasOne(u => u.Contact)
                .WithOne(c => c.User)
                .HasForeignKey<User>(u => u.Id);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            // Configures the Role enum to be stored as an integer in the database,
            // which matches your existing schema.
            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<int>();
        }
    }
}
