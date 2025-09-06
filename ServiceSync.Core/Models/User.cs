using ServiceSync.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    [ForeignKey("Id")]
    public virtual Contact Contact { get; set; }
    public string PasswordHash { get; set; }
    public bool IsEmailVerified { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerificationTokenExpiresAt { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}