using Microsoft.EntityFrameworkCore;
using ServiceSync.Core.Models;
using ServiceSync.Infrastructure.Context;
using ServiceSync.WebApi.Services;
using System.Security.Claims;

namespace ServiceSync.WebApi.GraphQL;

public class Mutation
{
    // --- AUTHENTICATION MUTATIONS ---
    public async Task<RegisterUserPayload> RegisterUserAsync(
        RegisterUserInput input,
        [Service] IAuthService authService)
    {
        try
        {
            var newUser = await authService.RegisterUserAsync(input.FirstName, input.LastName, input.Email, input.Password);
            return new RegisterUserPayload(newUser.Id);
        }
        catch (Exception ex)
        {
            throw new GraphQLException(new Error(ex.Message, "REGISTRATION_FAILED"));
        }
    }

    public async Task<LoginPayload> LoginAsync(
        LoginInput input,
        [Service] IAuthService authService)
    {
        var token = await authService.LoginUserAsync(input.Email, input.Password);
        if (token is null)
        {
            throw new GraphQLException(new Error("Invalid email or password.", "INVALID_CREDENTIALS"));
        }
        return new LoginPayload(token);
    }

    // --- COMPANY-USER LINKING MUTATIONS ---
    public async Task<LinkUserToCompanyPayload> LinkUserToCompanyAsync(
        LinkUserToCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var linkExists = await context.CompanyUsers
            .AnyAsync(cu => cu.CompanyId == input.CompanyId && cu.UserId == input.UserId);

        if (linkExists)
        {
            // Instead of throwing an error, let's just update the existing role.
            var existingLink = await context.CompanyUsers.FirstAsync(cu => cu.CompanyId == input.CompanyId && cu.UserId == input.UserId);
            existingLink.Role = input.Role;
            await context.SaveChangesAsync();
            return new LinkUserToCompanyPayload(existingLink);
        }

        // --- START OF FIX ---
        // Create the new CompanyUser link with the specified role.
        var companyUser = new CompanyUser
        {
            CompanyId = input.CompanyId,
            UserId = input.UserId,
            Role = input.Role // Set the role from the input
        };
        // --- END OF FIX ---

        context.CompanyUsers.Add(companyUser);
        await context.SaveChangesAsync();
        return new LinkUserToCompanyPayload(companyUser);
    }

    public async Task<UnlinkUserFromCompanyPayload> UnlinkUserFromCompanyAsync(
        UnlinkUserFromCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var companyUser = await context.CompanyUsers
            .FirstOrDefaultAsync(cu => cu.CompanyId == input.CompanyId && cu.UserId == input.UserId);

        if (companyUser is null)
        {
            return new UnlinkUserFromCompanyPayload(false, "Link not found.");
        }

        context.CompanyUsers.Remove(companyUser);
        await context.SaveChangesAsync();
        return new UnlinkUserFromCompanyPayload(true, "User unlinked successfully.");
    }

    // --- (The rest of the file remains the same) ---

    // --- COMPANY MUTATIONS ---
    public async Task<AddCompanyPayload> AddCompanyAsync(
        AddCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new GraphQLException(new Error("User is not authenticated or user ID is invalid.", "UNAUTHENTICATED"));
        }

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
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

            var companyUserLink = new CompanyUser
            {
                CompanyId = company.Id,
                UserId = userId,
                Role = Core.Enums.Role.Admin // The creator of a company is an Admin
            };
            context.CompanyUsers.Add(companyUserLink);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new AddCompanyPayload(company);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new GraphQLException($"An error occurred while creating the company: {ex.Message}");
        }
    }

    public async Task<UpdateCompanyPayload> UpdateCompanyAsync(
        UpdateCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var company = await context.Companies.FindAsync(input.CompanyId) ?? throw new GraphQLException(new Error("Company not found.", "COMPANY_NOT_FOUND"));

        company.Name = input.Name ?? company.Name;
        company.AddressLine1 = input.AddressLine1 ?? company.AddressLine1;
        company.AddressLine2 = input.AddressLine2 ?? company.AddressLine2;
        company.City = input.City ?? company.City;
        company.State = input.State ?? company.State;
        company.ZipCode = input.ZipCode ?? company.ZipCode;
        company.PhoneNumber = input.PhoneNumber ?? company.PhoneNumber;
        company.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return new UpdateCompanyPayload(company);
    }

    public async Task<DeletePayload> DeleteCompanyAsync(
        Guid id,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var company = await context.Companies.FindAsync(id);
        if (company is null)
        {
            return new DeletePayload(false, "Company not found.");
        }

        context.Companies.Remove(company);
        await context.SaveChangesAsync();
        return new DeletePayload(true, "Company deleted successfully.");
    }
}

