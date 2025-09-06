using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSync.Core.Models;

namespace ServiceSync.Infrastructure.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(255);

            // This sets up the one-to-one relationship where a Contact
            // can optionally have a User record. The User's primary key
            // is also its foreign key to the Contact.
            builder.HasOne(c => c.User)
                .WithOne(u => u.Contact)
                .HasForeignKey<User>(u => u.Id);

            // Defines the one-to-many relationship between Contact and CompanyResource
            builder.HasMany(c => c.ResourceCompanies)
                .WithOne(cr => cr.Resource)
                .HasForeignKey(cr => cr.ResourceId);

            // Defines the one-to-many relationship between Contact and CompanyClient
            builder.HasMany(c => c.ClientCompanies)
                .WithOne(cc => cc.Client)
                .HasForeignKey(cc => cc.ClientId);

            // Defines the one-to-many relationship for jobs created by this contact
            builder.HasMany(c => c.CreatedJobRequests)
                .WithOne(jr => jr.Client)
                .HasForeignKey(jr => jr.ClientId);

            // Defines the one-to-many relationship for jobs this contact is assigned to
            builder.HasMany(c => c.JobAssignments)
                .WithOne(rjr => rjr.Resource)
                .HasForeignKey(rjr => rjr.ResourceId);
        }
    }
}
