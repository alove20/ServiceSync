using Microsoft.EntityFrameworkCore;
using ServiceSync.Data.Context;
using ServiceSync.Data.Models;

namespace ServiceSync.WebAPI.GraphQL;

public class Mutation
{
    public async Task<AddCompanyPayload> AddCompanyAsync(
        AddCompanyInput input,
        ServiceSyncDbContext context)
    {
        var company = new Company
        {
            Name = input.Name,
            AddressLine1 = input.AddressLine1,
            AddressLine2 = input.AddressLine2,
            City = input.City,
            State = input.State,
            ZipCode = input.ZipCode,
            PhoneNumber = input.PhoneNumber,
            LogoUrl = input.LogoUrl
        };

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        return new AddCompanyPayload(company);
    }

    public async Task<UpdateCompanyPayload> UpdateCompanyAsync(
        UpdateCompanyInput input,
        ServiceSyncDbContext context)
    {
        var company = await context.Companies.FindAsync(input.CompanyId) ?? throw new GraphQLException(new Error("Company not found.", "COMPANY_NOT_FOUND"));
        company.Name = input.Name ?? company.Name;
        company.AddressLine1 = input.AddressLine1 ?? company.AddressLine1;
        company.City = input.City ?? company.City;
        company.State = input.State ?? company.State;
        company.ZipCode = input.ZipCode ?? company.ZipCode;
        company.PhoneNumber = input.PhoneNumber ?? company.PhoneNumber;

        await context.SaveChangesAsync();

        return new UpdateCompanyPayload(company);
    }

    public async Task<DeleteCompanyPayload> DeleteCompanyAsync(
        DeleteCompanyInput input,
        ServiceSyncDbContext context)
    {
        var company = await context.Companies.FindAsync(input.CompanyId);

        if (company is null)
        {
            return new DeleteCompanyPayload(false, "Company not found.");
        }

        context.Companies.Remove(company);
        await context.SaveChangesAsync();

        return new DeleteCompanyPayload(true, "Company successfully deleted.");
    }

    public async Task<AddUserPayload> AddUserAsync(
        AddUserInput input,
        ServiceSyncDbContext context)
    {
        var user = new User
        {
            Id = input.Id,
            PasswordHash = "good_password"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new AddUserPayload(user);
    }

    public async Task<UpdateUserPayload> UpdateUserAsync(
        UpdateUserInput input,
        ServiceSyncDbContext context)
    {
        var user = await context.Users.FindAsync(input.UserId) ?? throw new GraphQLException(new Error("User not found.", "USER_NOT_FOUND"));
        user.PasswordHash = input.Password;

        await context.SaveChangesAsync();

        return new UpdateUserPayload(user);
    }

    public async Task<DeleteUserPayload> DeleteUserAsync(
        DeleteUserInput input,
        ServiceSyncDbContext context)
    {
        var user = await context.Users.FindAsync(input.UserId);

        if (user is null)
        {
            return new DeleteUserPayload(false, "User not found.");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return new DeleteUserPayload(true, "User successfully deleted.");
    }

    public async Task<LinkUserToCompanyPayload> LinkUserToCompanyAsync(
        LinkUserToCompanyInput input,
        ServiceSyncDbContext context)
    {
        var exists = await context.CompanyUsers
           .AnyAsync(cu => cu.CompanyId == input.CompanyId && cu.ContactId == input.ContactId);

        if (exists)
        {
            throw new GraphQLException(new Error("User is already linked to this company.", "LINK_ALREADY_EXISTS"));
        }

        var companyUser = new CompanyUser
        {
            CompanyId = input.CompanyId,
            ContactId = input.ContactId
        };

        context.CompanyUsers.Add(companyUser);
        await context.SaveChangesAsync();

        var company = await context.Companies.FindAsync(input.CompanyId);
        var user = await context.Users.FindAsync(input.ContactId);

        return new LinkUserToCompanyPayload(company, user);
    }

    public async Task<UnlinkUserFromCompanyPayload> UnlinkUserFromCompanyAsync(
        UnlinkUserFromCompanyInput input,
        ServiceSyncDbContext context)
    {
        var companyUser = await context.CompanyUsers
           .FirstOrDefaultAsync(cu => cu.CompanyId == input.CompanyId && cu.ContactId == input.ContactId);

        if (companyUser is null)
        {
            return new UnlinkUserFromCompanyPayload(false, "Link between user and company not found.");
        }

        context.CompanyUsers.Remove(companyUser);
        await context.SaveChangesAsync();

        return new UnlinkUserFromCompanyPayload(true, "User successfully unlinked from company.");
    }
}