using System.Collections;
using Vigig.Domain.Entities;

namespace Vigig.Service.Models.Response.Authentication;

public class AuthResponse
{
    public string Name { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = new List<string>();
    public TokenResponse Token { get; set; } = null!;
}