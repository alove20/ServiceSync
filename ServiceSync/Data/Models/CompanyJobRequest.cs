namespace ServiceSync.Data.Models;

public class CompanyJobRequest
{
    public Guid CompanyId { get; set; }
    public virtual Company Company { get; set; } = null!;
    public Guid JobRequestId { get; set; }
    public virtual JobRequest JobRequest { get; set; } = null!;
}