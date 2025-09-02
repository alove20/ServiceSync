using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Core.Models;
public class Invoice
{
    [Key]
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid JobRequestId { get; set; }
    public DateTime EstimateReady { get; set; }
    public DateTime? EstimateApproved { get; set; }
    public string? EstimateApprovedIP { get; set; }
    public DateTime? PaymentDueDate { get; set; }
    public ICollection<JobRequestInvoice> JobRequests { get; set; } = [];
    public ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = [];
    public DateTime CreateAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
