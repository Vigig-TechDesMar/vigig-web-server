namespace Vigig.Api.Settings;

public class JwtSetting
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SigningKey { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public bool ValidateLifetime { get; set; }
    public int AccessTokenLifetimeInMinutes { get; set; }
    public int RefreshTokenLifetimeInMinutes { get; set; }
}