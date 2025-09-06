using ServiceSync.Core.Models;
using System.ComponentModel.DataAnnotations;

public class Contact
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<CompanyResource> ResourceCompanies { get; set; } = new List<CompanyResource>();
    public virtual ICollection<CompanyClient> ClientCompanies { get; set; } = new List<CompanyClient>();
    public virtual ICollection<JobRequest> CreatedJobRequests { get; set; } = new List<JobRequest>();
    public virtual ICollection<ResourceJobRequest> JobAssignments { get; set; } = new List<ResourceJobRequest>();
}