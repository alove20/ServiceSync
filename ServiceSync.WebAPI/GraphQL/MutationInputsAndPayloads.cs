// --- INPUT AND PAYLOAD RECORDS ---

using ServiceSync.Core.Models;

namespace ServiceSync.WebApi.GraphQL;

public record DeletePayload(bool Success, string Message);

// Company
public record AddCompanyInput(string Name, string? AddressLine1, string? AddressLine2, string? City, string? State, string? ZipCode, string? PhoneNumber, string? LogoUrl);
public record AddCompanyPayload(Company Company);
public record UpdateCompanyInput(Guid CompanyId, string? Name, string? AddressLine2, string? AddressLine1, string? City, string? State, string? ZipCode, string? PhoneNumber);
public record UpdateCompanyPayload(Company Company);

// Contact
public record AddContactInput(string FirstName, string LastName, string Email);
public record AddContactPayload(Contact Contact);
public record UpdateContactInput(Guid Id, string? FirstName, string? LastName, string? Email);
public record UpdateContactPayload(Contact Contact);

// JobRequest
public record AddJobRequestInput(string Title, string Description, Guid ClientId);
public record AddJobRequestPayload(JobRequest JobRequest);
public record UpdateJobRequestInput(Guid Id, string? Title, string? Description, Guid? ClientId);
public record UpdateJobRequestPayload(JobRequest JobRequest);

// Invoice
public record InvoiceLineItemInput(Guid LineItemId, int Quantity, decimal? PriceOverride, string? Notes);
public record AddInvoiceInput(Guid JobRequestId, Guid CreatorId, DateTime? PaymentDueDate, List<InvoiceLineItemInput>? LineItems);
public record AddInvoicePayload(Invoice Invoice);

