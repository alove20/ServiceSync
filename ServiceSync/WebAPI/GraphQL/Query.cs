using ServiceSync.Data.Context;
using ServiceSync.Data.Models;

namespace ServiceSync.WebAPI.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    public IQueryable<Company> GetCompanies(ServiceSyncDbContext context) =>
        context.Companies;

    [UseProjection]
    [UseFiltering]
    public IQueryable<User> GetUsers(ServiceSyncDbContext context) =>
        context.Users;

    [UseProjection]
    [UseFiltering]
    public IQueryable<Contact> GetContacts(ServiceSyncDbContext context) =>
        context.Contacts;
}