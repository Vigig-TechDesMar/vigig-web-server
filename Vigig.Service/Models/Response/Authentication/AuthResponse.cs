using Vigig.Domain.Entities;

namespace Vigig.Service.Models.Response.Authentication;

public class AuthResponse
{
    public string Name { get; set; } = null!;
    public ICollection<VigigRole> Role { get; set; } = Array.Empty<VigigRole>();
    public TokenResponse Token { get; set; } = null!;
}