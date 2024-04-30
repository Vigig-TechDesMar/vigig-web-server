namespace Vigig.Service.Models.Response.Authentication;

public class AuthResponse
{
    public string Name { get; set; }
    public string Role { get; set; }
    public TokenResponse Token { get; set; }
}