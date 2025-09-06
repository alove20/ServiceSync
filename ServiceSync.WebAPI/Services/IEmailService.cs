namespace ServiceSync.WebApi.Services;

/// <summary>
/// Defines the contract for a service that sends emails.
/// Using an interface allows us to easily swap out the implementation
/// (e.g., from a fake local service to a real one like SendGrid)
/// without changing our application's business logic.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="htmlBody">The HTML content of the email body.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    Task SendEmailAsync(string to, string subject, string htmlBody);
}
