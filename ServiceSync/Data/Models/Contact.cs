using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Data.Models;
public class Contact
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    public virtual ICollection<CompanyUser> Companies { get; set; } = [];
    public virtual ICollection<UserJobRequest> JobRequests { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
