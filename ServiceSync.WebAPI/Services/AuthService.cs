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
using System.Text.Json; // 1. Add this for JSON serialization

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
            Role = Role.User, // New users get the global 'User' role
            Contact = newContact
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        return newUser;
    }

    public async Task<string?> LoginUserAsync(string email, string password)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        // 2. Eagerly load the User and their CompanyUser links
        var contact = await context.Contacts
            .Include(c => c.User)
            .Include(c => c.UserCompanies) // <-- This is the key addition
            .FirstOrDefaultAsync(c => c.Email == email);

        if (contact?.User?.PasswordHash == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, contact.User.PasswordHash)) return null;

        // 3. Pass the full contact object (with company links) to the token generator
        return GenerateJwtToken(contact.User, contact);
    }

    private string GenerateJwtToken(User user, Contact contact)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        // 4. Create a list of company-specific roles
        var companyRoles = contact.UserCompanies
            .Select(cu => new { CompanyId = cu.CompanyId.ToString(), Role = cu.Role.ToString() })
            .ToList();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, contact.Email),
            new Claim(JwtRegisteredClaimNames.Name, $"{contact.FirstName} {contact.LastName}"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // 5. Add the user's GLOBAL role (e.g., "SuperUser")
            new Claim(ClaimTypes.Role, user.Role.ToString()), 
            // 6. Add the list of company-specific roles as a custom JSON claim
            new Claim("company_roles", JsonSerializer.Serialize(companyRoles), JsonClaimValueTypes.JsonArray)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8), // Extend token lifetime
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

