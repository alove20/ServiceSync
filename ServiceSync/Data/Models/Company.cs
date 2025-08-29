using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Data.Models;

public class Company
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? AddressLine1 { get; set; }
    [MaxLength(255)]
    public string? AddressLine2 { get; set; }
    [MaxLength(100)]
    public string? City { get; set; }
    [MaxLength(50)]
    public string? State { get; set; }
    [MaxLength(20)]
    public string? ZipCode { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    public string? LogoUrl { get; set; }
    public virtual ICollection<CompanyUser> Users { get; set; } = [];
    public virtual ICollection<CompanyClient> Clients { get; set; } = [];
    public virtual ICollection<CompanyJobRequest> JobRequests { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}