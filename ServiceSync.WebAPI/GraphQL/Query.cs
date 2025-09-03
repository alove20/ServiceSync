using Microsoft.EntityFrameworkCore;
using ServiceSync.Core.Models;
using ServiceSync.Infrastructure.Context;
using System.Security.Claims;

namespace ServiceSync.WebApi.GraphQL;

public class Query
{
    // --- GetCompanies (Already Secured) ---
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Company> GetCompanies(
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var context = contextFactory.CreateDbContext();
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null || !user.Identity.IsAuthenticated)
        {
            return new List<Company>().AsQueryable();
        }

        if (user.IsInRole("SuperUser"))
        {
            return context.Companies.AsNoTracking();
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new List<Company>().AsQueryable();
        }

        return context.Companies
            .Where(c => c.Users.Any(cu => cu.UserId == userId))
            .AsNoTracking();
    }

    // --- GetJobRequests (Already Secured) ---
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<JobRequest> GetJobRequests(
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var context = contextFactory.CreateDbContext();
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null || !user.Identity.IsAuthenticated)
        {
            return new List<JobRequest>().AsQueryable();
        }

        if (user.IsInRole("SuperUser"))
        {
            return context.JobRequests.AsNoTracking();
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new List<JobRequest>().AsQueryable();
        }

        var accessibleCompanyIds = context.CompanyUsers
            .Where(cu => cu.UserId == userId)
            .Select(cu => cu.CompanyId);

        return context.JobRequests
            .Where(jr => jr.Companies.Any(cjr => accessibleCompanyIds.Contains(cjr.CompanyId)))
            .AsNoTracking();
    }

    // --- GetInvoices (Newly Secured) ---
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Invoice> GetInvoices(
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var context = contextFactory.CreateDbContext();
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null || !user.Identity.IsAuthenticated)
        {
            return new List<Invoice>().AsQueryable();
        }

        if (user.IsInRole("SuperUser"))
        {
            return context.Invoices.AsNoTracking();
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new List<Invoice>().AsQueryable();
        }

        var accessibleCompanyIds = context.CompanyUsers
            .Where(cu => cu.UserId == userId)
            .Select(cu => cu.CompanyId);

        return context.Invoices
            .Where(i => i.JobRequest.Companies.Any(cjr => accessibleCompanyIds.Contains(cjr.CompanyId)))
            .AsNoTracking();
    }

    // --- Other Unsecured Queries ---
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Contact> GetContacts(ServiceSyncDbContext context) =>
        context.Contacts.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LineItem> GetLineItems(ServiceSyncDbContext context) =>
        context.LineItems.AsNoTracking();
}

