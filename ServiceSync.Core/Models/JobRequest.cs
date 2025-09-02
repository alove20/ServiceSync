using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Core.Models;

public class JobRequest
{
    [Key]
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? OpenedAt { get; set; }
    public virtual ICollection<JobRequestInvoice> Invoices { get; set; } = [];
    public virtual ICollection<UserJobRequest> Users { get; set; } = [];
    public virtual ICollection<CompanyJobRequest> Companies { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}