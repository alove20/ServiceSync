using ServiceSync.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;

public class CompanyResource
{
    public Guid CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    public virtual Company Company { get; set; }
    public Guid ResourceId { get; set; }
    [ForeignKey("ResourceId")]
    public virtual Contact Resource { get; set; }
    public Enums.Role Role { get; set; }
}

