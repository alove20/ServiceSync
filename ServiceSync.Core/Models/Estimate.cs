using ServiceSync.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;
public class Estimate
{
    [Key]
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    [ForeignKey("CreatorId")]
    public virtual Contact? Creator { get; set; }
    public Guid JobRequestId { get; set; }
    [ForeignKey("JobRequestId")]
    public virtual JobRequest? JobRequest { get; set; }
    public DateTime? EstimateReady { get; set; }
    public DateTime? EstimateApproved { get; set; }
    public string? EstimateApprovedIP { get; set; }
    public DateTime? PaymentDueDate { get; set; }
    public EstimateStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<EstimateLineItem> EstimateLineItems { get; set; } = new List<EstimateLineItem>();
}
