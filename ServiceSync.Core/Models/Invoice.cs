using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Core.Models;
public class Invoice
{
    [Key]
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid JobRequestId { get; set; }
    public virtual JobRequest? JobRequest { get; set; }
    public DateTime? EstimateReady { get; set; }
    public DateTime? EstimateApproved { get; set; }
    public string? EstimateApprovedIP { get; set; }
    public DateTime? PaymentDueDate { get; set; }
    public virtual ICollection<JobRequestInvoice> JobRequests { get; set; } = [];
    public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
