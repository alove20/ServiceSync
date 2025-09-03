using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Core.Models;
public class Contact
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<CompanyUser> UserCompanies { get; set; } = [];
    public virtual ICollection<CompanyClient> ClientCompanies { get; set; } = [];
    public virtual ICollection<UserJobRequest> JobRequests { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
