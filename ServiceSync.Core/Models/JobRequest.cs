using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;

public class JobRequest
{
    [Key]
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    [ForeignKey("ClientId")]
    public virtual Contact Client { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? OpenedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<CompanyJobRequest> Companies { get; set; } = new List<CompanyJobRequest>();
    public virtual ICollection<ResourceJobRequest> Resources { get; set; } = new List<ResourceJobRequest>();
    public virtual ICollection<JobRequestEstimate> Estimates { get; set; } = new List<JobRequestEstimate>();
}