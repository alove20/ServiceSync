using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;

public class ResourceJobRequest
{
    public Guid ResourceId { get; set; }
    [ForeignKey("ResourceId")]
    public virtual Contact Resource { get; set; }
    public Guid JobRequestId { get; set; }
    [ForeignKey("JobRequestId")]
    public virtual JobRequest JobRequest { get; set; }
}