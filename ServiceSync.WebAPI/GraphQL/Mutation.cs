using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using ServiceSync.Core.Enums;
using ServiceSync.Core.Models;
using ServiceSync.Infrastructure.Context;
using ServiceSync.WebApi.Services;
using System.Security.Claims;

namespace ServiceSync.WebApi.GraphQL;

public class Mutation
{
    // User Authentication
    [AllowAnonymous]
    public async Task<RegisterUserPayload> RegisterUserAsync(
        RegisterUserInput input,
        [Service] IAuthService authService)
    {
        var user = await authService.RegisterUserAsync(input.FirstName, input.LastName, input.Email, input.Password);
        return new RegisterUserPayload(user.Id);
    }

    [AllowAnonymous]
    public async Task<LoginPayload> LoginAsync(
        LoginInput input,
        [Service] IAuthService authService)
    {
        var token = await authService.LoginUserAsync(input.Email, input.Password);

        if (token is null)
        {
            throw new GraphQLException("Invalid email or password.");
        }

        return new LoginPayload(token);
    }

    // Company Management
    [Authorize]
    public async Task<AddCompanyPayload> AddCompanyAsync(
        AddCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new GraphQLException("User not authenticated.");
        }

        var newCompany = new Company
        {
            Name = input.Name,
            Email = input.Email,
            AddressLine1 = input.AddressLine1,
            AddressLine2 = input.AddressLine2,
            City = input.City,
            State = input.State,
            ZipCode = input.ZipCode,
            PhoneNumber = input.PhoneNumber,
            LogoUrl = input.LogoUrl,
            IsActive = true
        };

        var companyResource = new CompanyResource
        {
            Company = newCompany,
            ResourceId = userId,
            Role = Role.Admin
        };

        context.Companies.Add(newCompany);
        context.CompanyResources.Add(companyResource);
        await context.SaveChangesAsync();

        return new AddCompanyPayload(newCompany);
    }

    [Authorize]
    public async Task<UpdateCompanyPayload> UpdateCompanyAsync(
        UpdateCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var company = await context.Companies.FindAsync(input.CompanyId);

        if (company is null)
        {
            throw new GraphQLException("Company not found.");
        }

        if (input.Name is not null) company.Name = input.Name;
        if (input.Email is not null) company.Email = input.Email;
        if (input.AddressLine1 is not null) company.AddressLine1 = input.AddressLine1;
        if (input.AddressLine2 is not null) company.AddressLine2 = input.AddressLine2;
        if (input.City is not null) company.City = input.City;
        if (input.State is not null) company.State = input.State;
        if (input.ZipCode is not null) company.ZipCode = input.ZipCode;
        if (input.PhoneNumber is not null) company.PhoneNumber = input.PhoneNumber;

        company.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new UpdateCompanyPayload(company);
    }

    [Authorize]
    public async Task<DeletePayload> DeleteCompanyAsync(
        Guid companyId,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var company = await context.Companies.FindAsync(companyId);
        if (company is null)
        {
            return new DeletePayload(false, "Company not found.");
        }

        company.IsActive = false; // Soft delete
        await context.SaveChangesAsync();

        return new DeletePayload(true, "Company archived successfully.");
    }

    // Contact Management
    [Authorize]
    public async Task<AddContactPayload> AddContactAsync(
        AddContactInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var existingContact = await context.Contacts.FirstOrDefaultAsync(c => c.Email == input.Email);
        if (existingContact != null)
        {
            throw new GraphQLException("A contact with this email already exists.");
        }

        var newContact = new Contact
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            AddressLine1 = input.AddressLine1,
            AddressLine2 = input.AddressLine2,
            City = input.City,
            State = input.State,
            ZipCode = input.ZipCode
        };

        context.Contacts.Add(newContact);
        await context.SaveChangesAsync();

        return new AddContactPayload(newContact);
    }

    [Authorize]
    public async Task<UpdateContactPayload> UpdateContactAsync(
        UpdateContactInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var contact = await context.Contacts.FindAsync(input.Id);

        if (contact is null)
        {
            throw new GraphQLException("Contact not found.");
        }

        contact.FirstName = input.FirstName ?? contact.FirstName;
        contact.LastName = input.LastName ?? contact.LastName;
        contact.Email = input.Email ?? contact.Email;
        contact.PhoneNumber = input.PhoneNumber ?? contact.PhoneNumber;
        contact.AddressLine1 = input.AddressLine1 ?? contact.AddressLine1;
        contact.AddressLine2 = input.AddressLine2 ?? contact.AddressLine2;
        contact.City = input.City ?? contact.City;
        contact.State = input.State ?? contact.State;
        contact.ZipCode = input.ZipCode ?? contact.ZipCode;
        contact.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new UpdateContactPayload(contact);
    }

    // Invoice Management
    [Authorize]
    public async Task<AddInvoicePayload> AddInvoiceAsync(
        CreateInvoiceInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var creatorId))
        {
            throw new GraphQLException("User is not authenticated.");
        }

        var newInvoice = new Invoice
        {
            JobRequestId = input.JobRequestId,
            CreatorId = creatorId,
            PaymentDueDate = input.PaymentDueDate,
            InvoiceLineItems = input.LineItems.Select(li => new InvoiceLineItem
            {
                LineItemId = li.LineItemId,
                Quantity = li.Quantity,
                PriceOverride = li.PriceOverride,
                Notes = li.Notes
            }).ToList()
        };

        context.Invoices.Add(newInvoice);
        await context.SaveChangesAsync();

        return new AddInvoicePayload(newInvoice);
    }

    // Job Request Management
    [Authorize]
    public async Task<AddJobRequestPayload> AddJobRequestAsync(
        AddJobRequestInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var jobRequest = new JobRequest
        {
            Title = input.Title,
            Description = input.Description,
            ClientId = input.ClientId
        };
        context.JobRequests.Add(jobRequest);
        await context.SaveChangesAsync();
        return new AddJobRequestPayload(jobRequest);
    }

    [Authorize]
    public async Task<AssignJobRequestToCompanyPayload> AssignJobRequestToCompanyAsync(
        AssignJobRequestToCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var jobRequest = await context.JobRequests.FindAsync(input.JobRequestId);
        if (jobRequest is null)
        {
            throw new GraphQLException("Job Request not found.");
        }

        var companyLink = new CompanyJobRequest
        {
            CompanyId = input.CompanyId,
            JobRequestId = input.JobRequestId
        };

        context.CompanyJobRequests.Add(companyLink);
        await context.SaveChangesAsync();

        return new AssignJobRequestToCompanyPayload(jobRequest);
    }

    [Authorize]
    public async Task<UpdateJobRequestPayload> UpdateJobRequestAsync(
        UpdateJobRequestInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var jobRequest = await context.JobRequests.FindAsync(input.Id);
        if (jobRequest is null)
        {
            throw new GraphQLException("Job Request not found.");
        }

        jobRequest.Title = input.Title ?? jobRequest.Title;
        jobRequest.Description = input.Description ?? jobRequest.Description;
        jobRequest.ClientId = input.ClientId ?? jobRequest.ClientId;
        jobRequest.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return new UpdateJobRequestPayload(jobRequest);
    }

    // Resource & Role Management
    [Authorize]
    public async Task<ImportContactAsClientPayload> ImportContactAsClientAsync(
        ImportContactAsClientInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var companyClient = new CompanyClient
        {
            CompanyId = input.CompanyId,
            ClientId = input.ContactId
        };
        context.CompanyClients.Add(companyClient);
        await context.SaveChangesAsync();
        return new ImportContactAsClientPayload(companyClient);
    }

    [Authorize]
    public async Task<ImportContactAsResourcePayload> ImportContactAsResourceAsync(
        ImportContactAsResourceInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var companyResource = new CompanyResource
        {
            CompanyId = input.CompanyId,
            ResourceId = input.ContactId,
            Role = input.Role
        };
        context.CompanyResources.Add(companyResource);
        await context.SaveChangesAsync();
        return new ImportContactAsResourcePayload(companyResource);
    }

    [Authorize]
    public async Task<AssignResourceToJobRequestPayload> AssignResourceToJobRequestAsync(
        AssignResourceToJobRequestInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var resourceJobRequest = new ResourceJobRequest
        {
            JobRequestId = input.JobRequestId,
            ResourceId = input.ResourceId
        };
        context.ResourceJobRequests.Add(resourceJobRequest);
        await context.SaveChangesAsync();
        return new AssignResourceToJobRequestPayload(resourceJobRequest);
    }

    [Authorize]
    public async Task<UnassignResourceFromJobRequestPayload> UnassignResourceFromJobRequestAsync(
        UnassignResourceFromJobRequestInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var resourceJobRequest = await context.ResourceJobRequests
            .FirstOrDefaultAsync(ujr => ujr.JobRequestId == input.JobRequestId && ujr.ResourceId == input.ResourceId);

        if (resourceJobRequest != null)
        {
            context.ResourceJobRequests.Remove(resourceJobRequest);
            await context.SaveChangesAsync();
        }

        return new UnassignResourceFromJobRequestPayload(true);
    }

    [Authorize]
    public async Task<LinkResourceToCompanyPayload> LinkResourceToCompanyAsync(
        LinkResourceToCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var existingLink = await context.CompanyResources
            .FirstOrDefaultAsync(cr => cr.CompanyId == input.CompanyId && cr.ResourceId == input.ResourceId);

        if (existingLink != null)
        {
            existingLink.Role = input.Role;
            await context.SaveChangesAsync();
            return new LinkResourceToCompanyPayload(existingLink);
        }
        else
        {
            var newLink = new CompanyResource
            {
                CompanyId = input.CompanyId,
                ResourceId = input.ResourceId,
                Role = input.Role
            };
            context.CompanyResources.Add(newLink);
            await context.SaveChangesAsync();
            return new LinkResourceToCompanyPayload(newLink);
        }
    }


    [Authorize]
    public async Task<UnlinkResourceFromCompanyPayload> UnlinkResourceFromCompanyAsync(
        UnlinkResourceFromCompanyInput input,
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory)
    {
        var context = await contextFactory.CreateDbContextAsync();
        var companyResource = await context.CompanyResources
            .FirstOrDefaultAsync(cr => cr.CompanyId == input.CompanyId && cr.ResourceId == input.ResourceId);

        if (companyResource is null)
        {
            return new UnlinkResourceFromCompanyPayload(false, "Resource link not found.");
        }

        context.CompanyResources.Remove(companyResource);
        await context.SaveChangesAsync();

        return new UnlinkResourceFromCompanyPayload(true, "Resource unlinked successfully.");
    }
}

