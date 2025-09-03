using ServiceSync.Infrastructure.Context;
using ServiceSync.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceSync.WebApi.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Company> GetCompanies(ServiceSyncDbContext context) =>
        context.Companies.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Contact> GetContacts(ServiceSyncDbContext context) =>
        context.Contacts.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<JobRequest> GetJobRequests(ServiceSyncDbContext context) =>
        context.JobRequests.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Invoice> GetInvoices(ServiceSyncDbContext context) =>
        context.Invoices.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LineItem> GetLineItems(ServiceSyncDbContext context) =>
        context.LineItems.AsNoTracking();
}
