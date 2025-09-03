using Microsoft.EntityFrameworkCore;
using ServiceSync.Core.Models;
using ServiceSync.Infrastructure.Context;

// Corrected namespace as per your instruction
namespace ServiceSync.WebApi.GraphQL;

public class Mutation
{
    // --- COMPANY MUTATIONS ---
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
        company.AddressLine2 = input.AddressLine2 ?? company.AddressLine2;
        company.City = input.City ?? company.City;
        company.State = input.State ?? company.State;
        company.ZipCode = input.ZipCode ?? company.ZipCode;
        company.PhoneNumber = input.PhoneNumber ?? company.PhoneNumber;
        company.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new UpdateCompanyPayload(company);
    }

    // Refined to accept a simple Guid for deletion
    public async Task<DeletePayload> DeleteCompanyAsync(
        Guid id,
        ServiceSyncDbContext context)
    {
        var company = await context.Companies.FindAsync(id);
        if (company is null)
        {
            return new DeletePayload(false, "Company not found.");
        }

        context.Companies.Remove(company);
        await context.SaveChangesAsync();
        return new DeletePayload(true, "Company deleted successfully.");
    }

    // --- CONTACT MUTATIONS ---
    public async Task<AddContactPayload> AddContactAsync(
        AddContactInput input,
        ServiceSyncDbContext context)
    {
        var contact = new Contact
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
        };

        context.Contacts.Add(contact);
        await context.SaveChangesAsync();

        return new AddContactPayload(contact);
    }
    public async Task<UpdateContactPayload> UpdateContactAsync(
       UpdateContactInput input,
       ServiceSyncDbContext context)
    {
        var contact = await context.Contacts.FindAsync(input.Id) ?? throw new GraphQLException(new Error("Contact not found.", "CONTACT_NOT_FOUND"));

        contact.FirstName = input.FirstName ?? contact.FirstName;
        contact.LastName = input.LastName ?? contact.LastName;
        contact.Email = input.Email ?? contact.Email;
        contact.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return new UpdateContactPayload(contact);
    }

    public async Task<DeletePayload> DeleteContactAsync(
       Guid id,
       ServiceSyncDbContext context)
    {
        var contact = await context.Contacts.FindAsync(id);
        if (contact is null)
        {
            return new DeletePayload(false, "Contact not found.");
        }

        context.Contacts.Remove(contact);
        await context.SaveChangesAsync();
        return new DeletePayload(true, "Contact deleted successfully.");
    }

    // --- JOB REQUEST MUTATIONS ---
    public async Task<AddJobRequestPayload> AddJobRequestAsync(
        AddJobRequestInput input,
        ServiceSyncDbContext context)
    {
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

    public async Task<UpdateJobRequestPayload> UpdateJobRequestAsync(
        UpdateJobRequestInput input,
        ServiceSyncDbContext context)
    {
        var jobRequest = await context.JobRequests.FindAsync(input.Id) ?? throw new GraphQLException(new Error("JobRequest not found.", "JOB_REQUEST_NOT_FOUND"));

        jobRequest.Title = input.Title ?? jobRequest.Title;
        jobRequest.Description = input.Description ?? jobRequest.Description;
        jobRequest.ClientId = input.ClientId ?? jobRequest.ClientId;
        jobRequest.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return new UpdateJobRequestPayload(jobRequest);
    }

    public async Task<DeletePayload> DeleteJobRequestAsync(
        Guid id,
        ServiceSyncDbContext context)
    {
        var jobRequest = await context.JobRequests.FindAsync(id);
        if (jobRequest is null)
        {
            return new DeletePayload(false, "JobRequest not found.");
        }

        context.JobRequests.Remove(jobRequest);
        await context.SaveChangesAsync();
        return new DeletePayload(true, "JobRequest deleted successfully.");
    }

    // --- INVOICE MUTATIONS ---
    public async Task<AddInvoicePayload> AddInvoiceAsync(
        AddInvoiceInput input,
        ServiceSyncDbContext context)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var jobRequest = await context.JobRequests.FindAsync(input.JobRequestId) ?? throw new GraphQLException("JobRequest not found.");

            var newInvoice = new Invoice
            {
                JobRequestId = input.JobRequestId,
                CreatorId = input.CreatorId,
                PaymentDueDate = input.PaymentDueDate
            };

            context.Invoices.Add(newInvoice);
            await context.SaveChangesAsync();

            if (input.LineItems != null && input.LineItems.Count != 0)
            {
                foreach (var lineItemInput in input.LineItems)
                {
                    var invoiceLineItem = new InvoiceLineItem
                    {
                        InvoiceId = newInvoice.Id,
                        LineItemId = lineItemInput.LineItemId,
                        Quantity = lineItemInput.Quantity,
                        PriceOverride = lineItemInput.PriceOverride,
                        Notes = lineItemInput.Notes
                    };
                    context.InvoiceLineItems.Add(invoiceLineItem);
                }
                await context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return new AddInvoicePayload(newInvoice);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new GraphQLException($"An error occurred while creating the invoice: {ex.Message}");
        }
    }
}