namespace ServiceSync.Core.Models;

public class CompanyClient
{
    public Guid CompanyId { get; set; }
    public virtual Company? Company { get; set; }
    public Guid ClientId { get; set; }
    public virtual Contact? Client { get; set; }
}