namespace ServiceSync.Data.Models;

public class JobRequestInvoice
{
    public Guid JobRequestId { get; set; }
    public virtual JobRequest JobRequest { get; set; } = null!;
    public Guid InvoiceId { get; set; }
    public virtual Invoice Invoice { get; set; } = null!;
}