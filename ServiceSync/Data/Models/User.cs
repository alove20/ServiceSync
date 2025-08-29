using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Data.Models;
public class User
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(255)]
    public string Username { get; set; } = string.Empty;
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
