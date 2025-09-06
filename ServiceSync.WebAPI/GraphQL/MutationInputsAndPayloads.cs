using ServiceSync.Core.Enums;
using ServiceSync.Core.Models;

namespace ServiceSync.WebApi.GraphQL
{
    // User Authentication
    public record RegisterUserInput(string FirstName, string LastName, string Email, string Password);
    public record RegisterUserPayload(Guid UserId);
    public record LoginInput(string Email, string Password);
    public record LoginPayload(string Token);

    // --- START OF CHANGE ---
    // Company Management
    public record AddCompanyInput(string Name, string? Email, string? AddressLine1, string? AddressLine2, string? City, string? State, string? ZipCode, string? PhoneNumber, string? LogoUrl);
    public record AddCompanyPayload(Company Company);
    public record UpdateCompanyInput(Guid CompanyId, string? Name, string? Email, string? AddressLine1, string? AddressLine2, string? City, string? State, string? ZipCode, string? PhoneNumber);
    public record UpdateCompanyPayload(Company Company);
    public record DeletePayload(bool Success, string Message);
    // --- END OF CHANGE ---


    // Contact Management
    public record AddContactInput(string FirstName, string LastName, string Email, string? PhoneNumber, string? AddressLine1, string? AddressLine2, string? City, string? State, string? ZipCode);
    public record AddContactPayload(Contact Contact);
    public record UpdateContactInput(Guid Id, string? FirstName, string? LastName, string? Email, string? PhoneNumber, string? AddressLine1, string? AddressLine2, string? City, string? State, string? ZipCode);
    public record UpdateContactPayload(Contact Contact);
    public record ImportContactAsClientInput(Guid ContactId, Guid CompanyId);
    public record ImportContactAsClientPayload(CompanyClient CompanyClient);
    public record ImportContactAsResourceInput(Guid ContactId, Guid CompanyId, Role Role);
    public record ImportContactAsResourcePayload(CompanyResource CompanyResource);


    // Job Request Management
    public record AddJobRequestInput(string Title, string Description, Guid ClientId);
    public record AddJobRequestPayload(JobRequest JobRequest);
    public record UpdateJobRequestInput(Guid Id, string? Title, string? Description, Guid? ClientId);
    public record UpdateJobRequestPayload(JobRequest JobRequest);
    public record AssignJobRequestToCompanyInput(Guid JobRequestId, Guid CompanyId);
    public record AssignJobRequestToCompanyPayload(JobRequest JobRequest);
    public record AssignResourceToJobRequestInput(Guid JobRequestId, Guid ResourceId);
    public record AssignResourceToJobRequestPayload(ResourceJobRequest ResourceJobRequest);
    public record UnassignResourceFromJobRequestInput(Guid JobRequestId, Guid ResourceId);
    public record UnassignResourceFromJobRequestPayload(bool Success);


    // Invoice Management
    public record InvoiceLineItemInput(Guid LineItemId, int Quantity, decimal? PriceOverride, string? Notes);
    public record CreateInvoiceInput(Guid JobRequestId, System.DateTime? PaymentDueDate, List<InvoiceLineItemInput> LineItems);
    public record CreateInvoicePayload(Invoice Invoice);
    public record AddInvoicePayload(Invoice Invoice);


    // Resource & Role Management (Legacy/Specific)
    public record LinkResourceToCompanyInput(Guid CompanyId, Guid ResourceId, Role Role);
    public record LinkResourceToCompanyPayload(CompanyResource companyResource);
    public record UnlinkResourceFromCompanyInput(Guid CompanyId, Guid ResourceId);
    public record UnlinkResourceFromCompanyPayload(bool Success, string Message);
}

