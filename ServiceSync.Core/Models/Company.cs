using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Core.Models;

public class Company
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<CompanyResource> Resources { get; set; } = new List<CompanyResource>();
    public virtual ICollection<CompanyClient> Clients { get; set; } = new List<CompanyClient>();
    public virtual ICollection<CompanyJobRequest> JobRequests { get; set; } = new List<CompanyJobRequest>();
    public virtual ICollection<CatalogItem> CatalogItems { get; set; } = new List<CatalogItem>();
    public virtual ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
    public virtual ICollection<EstimateTemplate> EstimateTemplates { get; set; } = new List<EstimateTemplate>();
}