namespace ServiceSync.WebApi.Settings;

// This class is used to map the JwtSettings section from appsettings.json
// into a strongly-typed object. This is a best practice that helps avoid
// typos and provides compile-time safety for your configuration.
public class JwtSettings
{
    // The secret key used to sign and verify JWT tokens.
    // IMPORTANT: This should be a long, random, and complex string.
    // In production, load this from a secure source like Azure Key Vault or environment variables.
    public string Secret { get; set; } = string.Empty;

    // The "issuer" of the token (your application).
    public string Issuer { get; set; } = string.Empty;

    // The "audience" of the token (also your application).
    public string Audience { get; set; } = string.Empty;
}