namespace ServiceSync.Core.Models;

public class CompanyClient
{
    public Guid CompanyId { get; set; }
    public virtual Company? Company { get; set; }
    public Guid ContactId { get; set; }
    public virtual Contact? Client { get; set; }
}