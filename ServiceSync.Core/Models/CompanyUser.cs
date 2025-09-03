using ServiceSync.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Core.Models;

public class CompanyUser
{
    public Guid CompanyId { get; set; }
    public virtual Company? Company { get; set; }
    public Guid UserId { get; set; }
    public virtual Contact? User { get; set; }
    [Required]
    public Role Role { get; set; } = Role.User;
}