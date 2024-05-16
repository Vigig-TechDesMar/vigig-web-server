namespace Vigig.Service.Models.Response.Authentication;

public class TokenResponse
{
    public required string AccessToken { get; set; } 
    public required string RefreshToken { get; set; }
    public DateTimeOffset ExpiresAt { get; set; } 
}