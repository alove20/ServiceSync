using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceSync.Core.Enums;
using ServiceSync.Core.Models;
using ServiceSync.Infrastructure.Context;
using ServiceSync.WebApi.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ServiceSync.WebApi.Services;

public class AuthService : IAuthService
{
    private readonly IDbContextFactory<ServiceSyncDbContext> _contextFactory;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IDbContextFactory<ServiceSyncDbContext> contextFactory, IOptions<JwtSettings> jwtSettings)
    {
        _contextFactory = contextFactory;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<User> RegisterUserAsync(string firstName, string lastName, string email, string password)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var existingContact = await context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
        if (existingContact != null)
        {
            throw new Exception("A user with this email address already exists.");
        }

        var newContact = new Contact { FirstName = firstName, LastName = lastName, Email = email };
        var newUser = new User
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = Role.User,
            Contact = newContact
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        return newUser;
    }

    public async Task<string?> LoginUserAsync(string email, string password)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        // --- START OF THE FIX ---
        // The query now includes the Company navigation property so we can check if it's active.
        var contact = await context.Contacts
            .Include(c => c.User)
            .Include(c => c.ResourceCompanies)
                .ThenInclude(cr => cr.Company)
            .FirstOrDefaultAsync(c => c.Email == email);
        // --- END OF THE FIX ---

        if (contact?.User?.PasswordHash == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, contact.User.PasswordHash)) return null;

        return GenerateJwtToken(contact.User, contact);
    }

    private string GenerateJwtToken(User user, Contact contact)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        // --- START OF THE FIX ---
        // Filter the roles to include only those from active companies before serializing.
        var companyRolesData = contact.ResourceCompanies
            .Where(cr => cr.Company.IsActive)
            .Select(cu => new { CompanyId = cu.CompanyId.ToString(), Role = cu.Role.ToString() })
            .ToList();
        // --- END OF THE FIX ---

        var companyRolesJson = JsonSerializer.Serialize(companyRolesData);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, contact.Email),
            new Claim(JwtRegisteredClaimNames.Name, $"{contact.FirstName} {contact.LastName}"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("company_roles", companyRolesJson)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

