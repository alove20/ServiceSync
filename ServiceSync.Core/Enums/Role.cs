namespace ServiceSync.Core.Enums;

/// <summary>
/// Defines the set of possible user roles within the application.
/// Using an enum provides type safety and prevents magic strings.
/// </summary>
public enum Role
{
    /// <summary>
    /// Standard user with limited, view-only permissions within a company.
    /// </summary>
    User = 0,

    /// <summary>
    /// A company-level administrator who can manage company info and users.
    /// </summary>
    Admin = 1,

    /// <summary>
    /// A global super-user who has unrestricted access to all data.
    /// </summary>
    SuperUser = 2
}