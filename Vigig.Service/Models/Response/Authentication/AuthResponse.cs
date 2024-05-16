using Vigig.Domain.Models;

namespace Vigig.Service.Models.Response.Authentication;

public class AuthResponse
{
    public string Name { get; set; }
    public ICollection<VigigRole> Role { get; set; }
    public TokenResponse Token { get; set; }
}