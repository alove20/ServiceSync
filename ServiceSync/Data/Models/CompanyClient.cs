namespace ServiceSync.Data.Models;

public class CompanyClient
{
    public Guid CompanyId { get; set; }
    public virtual Company Company { get; set; } = null!;
    public Guid ContactId { get; set; }
    public virtual Contact Client { get; set; } = null!;
}