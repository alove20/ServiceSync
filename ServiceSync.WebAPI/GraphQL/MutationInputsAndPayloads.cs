using ServiceSync.Core.Models;
using ServiceSync.Core.Enums; // 1. Import the Enums namespace

namespace ServiceSync.WebApi.GraphQL;

// --- AUTHENTICATION RECORDS ---
public record RegisterUserInput(string FirstName, string LastName, string Email, string Password);
public record RegisterUserPayload(Guid UserId);
public record LoginInput(string Email, string Password);
public record LoginPayload(string Token);

// --- COMPANY-USER LINKING RECORDS ---
// 2. Add the Role parameter to the input record.
public record LinkUserToCompanyInput(Guid CompanyId, Guid UserId, Role Role);
public record LinkUserToCompanyPayload(CompanyUser companyUser);
public record UnlinkUserFromCompanyInput(Guid CompanyId, Guid UserId);
public record UnlinkUserFromCompanyPayload(bool Success, string Message);


// --- EXISTING RECORDS ---
public record DeletePayload(bool Success, string Message);
// ... (rest of records)
public record AddCompanyInput(string Name, string? AddressLine1, string? AddressLine2, string? City, string? State, string? ZipCode, string? PhoneNumber, string? LogoUrl);
public record AddCompanyPayload(Company Company);
public record UpdateCompanyInput(Guid CompanyId, string? Name, string? AddressLine2, string? AddressLine1, string? City, string? State, string? ZipCode, string? PhoneNumber);
public record UpdateCompanyPayload(Company Company);
public record AddContactInput(string FirstName, string LastName, string Email);
public record AddContactPayload(Contact Contact);
public record UpdateContactInput(Guid Id, string? FirstName, string? LastName, string? Email);
public record UpdateContactPayload(Contact Contact);
public record AddJobRequestInput(string Title, string Description, Guid ClientId);
public record AddJobRequestPayload(JobRequest JobRequest);
public record UpdateJobRequestInput(Guid Id, string? Title, string? Description, Guid? ClientId);
public record UpdateJobRequestPayload(JobRequest JobRequest);
public record InvoiceLineItemInput(
    Guid LineItemId,
    int Quantity,
    decimal? PriceOverride,
    string? Notes
);
public record AddInvoiceInput(
    Guid JobRequestId,
    Guid CreatorId,
    DateTime? PaymentDueDate,
    List<InvoiceLineItemInput>? LineItems
);
public record AddInvoicePayload(Invoice Invoice);

