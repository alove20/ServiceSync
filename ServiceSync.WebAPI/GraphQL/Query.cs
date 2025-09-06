using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using ServiceSync.Core.Models;
using ServiceSync.Infrastructure.Context;
using System.Security.Claims;

namespace ServiceSync.WebApi.GraphQL;

public class Query
{
    // --- GetCompanies (Updated with IsActive filter) ---
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

        var companiesQuery = context.Companies.Where(c => c.IsActive);

        if (user.IsInRole("SuperUser"))
        {
            return companiesQuery.AsNoTracking();
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new List<Company>().AsQueryable();
        }

        return companiesQuery
            .Where(c => c.Resources.Any(cu => cu.ResourceId == userId))
            .AsNoTracking();
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<JobRequest> GetJobRequestsForCurrentUser(
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

        var accessibleCompanyIds = context.CompanyResources
            .Where(cu => cu.ResourceId == userId)
            .Select(cu => cu.CompanyId);

        return context.JobRequests
            .Where(jr => jr.Companies.Any(cjr => accessibleCompanyIds.Contains(cjr.CompanyId)))
            .AsNoTracking();
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<JobRequest> GetJobRequestById(
    [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory,
    Guid id)
    {
        var context = contextFactory.CreateDbContext();
        return context.JobRequests.Where(jr => jr.Id == id).AsNoTracking();
    }

    // --- START OF REFACTOR ---
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Estimate> GetEstimates(
        [Service] IDbContextFactory<ServiceSyncDbContext> contextFactory,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var context = contextFactory.CreateDbContext();
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null || !user.Identity.IsAuthenticated)
        {
            return new List<Estimate>().AsQueryable();
        }

        if (user.IsInRole("SuperUser"))
        {
            return context.Estimates.AsNoTracking();
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new List<Estimate>().AsQueryable();
        }

        var accessibleCompanyIds = context.CompanyResources
            .Where(cu => cu.ResourceId == userId)
            .Select(cu => cu.CompanyId);

        return context.Estimates
            .Where(e => e.JobRequest.Companies.Any(cjr => accessibleCompanyIds.Contains(cjr.CompanyId)))
            .AsNoTracking();
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<CatalogItem> GetCatalogItems([Service] IDbContextFactory<ServiceSyncDbContext> contextFactory) =>
        contextFactory.CreateDbContext().CatalogItems.AsNoTracking();

    [Authorize]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ItemCategory> GetItemCategories([Service] IDbContextFactory<ServiceSyncDbContext> contextFactory) =>
        contextFactory.CreateDbContext().ItemCategories.AsNoTracking();

    [Authorize]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EstimateTemplate> GetEstimateTemplates([Service] IDbContextFactory<ServiceSyncDbContext> contextFactory) =>
        contextFactory.CreateDbContext().EstimateTemplates.AsNoTracking();
    // --- END OF REFACTOR ---

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Contact> GetContacts([Service] IDbContextFactory<ServiceSyncDbContext> contextFactory) =>
        contextFactory.CreateDbContext().Contacts.AsNoTracking();
}

