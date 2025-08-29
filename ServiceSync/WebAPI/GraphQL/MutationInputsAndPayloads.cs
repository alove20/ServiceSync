using ServiceSync.Data.Models;

namespace ServiceSync.WebAPI.GraphQL;

public record AddCompanyInput(string Name, string? AddressLine1, string? AddressLine2, string? City, string? State, string? ZipCode, string? PhoneNumber, string? LogoUrl);
public record AddCompanyPayload(Company Company);
public record UpdateCompanyInput(Guid CompanyId, string? Name, string? AddressLine1, string? City, string? State, string? ZipCode, string? PhoneNumber);
public record UpdateCompanyPayload(Company Company);
public record DeleteCompanyInput(Guid CompanyId);
public record DeleteCompanyPayload(bool Success, string Message);
public record AddUserInput(Guid Id, string Password);
public record AddUserPayload(User User);
public record UpdateUserInput(Guid UserId, string Password);
public record UpdateUserPayload(User User);
public record DeleteUserInput(Guid UserId);
public record DeleteUserPayload(bool Success, string Message);
public record LinkUserToCompanyInput(Guid CompanyId, Guid ContactId);
public record LinkUserToCompanyPayload(Company? Company, User? User);
public record UnlinkUserFromCompanyInput(Guid CompanyId, Guid ContactId);
public record UnlinkUserFromCompanyPayload(bool Success, string Message);