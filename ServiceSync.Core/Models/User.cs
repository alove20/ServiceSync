using ServiceSync.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public Role Role { get; set; } = Role.User;

    public bool IsEmailVerified { get; set; } = false;

    [MaxLength(255)]
    public string? VerificationToken { get; set; }

    public DateTime? VerificationTokenExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property back to the contact details
    public virtual Contact? Contact { get; set; }
}