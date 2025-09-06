using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;

public class CompanyJobRequest
{
    public Guid CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    public virtual Company Company { get; set; }
    public Guid JobRequestId { get; set; }
    [ForeignKey("JobRequestId")]
    public virtual JobRequest JobRequest { get; set; }
}