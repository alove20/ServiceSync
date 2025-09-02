using ServiceSync.Infrastructure.Context;
using ServiceSync.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ServiceSync.WebAPI.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    public IQueryable<Company> GetCompanies(ServiceSyncDbContext context) =>
        context.Companies.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    public IQueryable<User> GetUsers(ServiceSyncDbContext context) =>
        context.Users.AsNoTracking();

    [UseProjection]
    [UseFiltering]
    public IQueryable<Contact> GetContacts(ServiceSyncDbContext context) =>
        context.Contacts.AsNoTracking();
}