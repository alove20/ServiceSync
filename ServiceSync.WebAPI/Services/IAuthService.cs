using ServiceSync.Core.Models;

namespace ServiceSync.WebApi.Services;

/// <summary>
/// Defines the contract for the authentication service, which handles
/// user registration, login, and JWT generation.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="email">The user's email address. Must be unique.</param>
    /// <param name="password">The user's plain-text password.</param>
    /// <returns>The newly created User object.</returns>
    Task<User> RegisterUserAsync(string firstName, string lastName, string email, string password);

    /// <summary>
    /// Authenticates a user based on their email and password.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's plain-text password.</param>
    /// <returns>A JWT token string if authentication is successful; otherwise, null.</returns>
    Task<string?> LoginUserAsync(string email, string password);
}
