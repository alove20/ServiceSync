using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;

/// <summary>
/// Represents the many-to-many relationship between a Company and a Contact who is a Client.
/// </summary>
public class CompanyClient
{
    public Guid CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    public virtual Company Company { get; set; }
    public Guid ClientId { get; set; }
    [ForeignKey("ClientId")]
    public virtual Contact Client { get; set; }
}
