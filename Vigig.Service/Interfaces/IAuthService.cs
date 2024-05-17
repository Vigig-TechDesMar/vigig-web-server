using Vigig.Common.Attribute;
using Vigig.Domain.Entities;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IAuthService 
{
    Task<ServiceActionResult> RegisterAsync(RegisterRequest request);
    Task<ServiceActionResult> LoginAsync(LoginRequest request);
    Task<ServiceActionResult> RefreshTokenAsync(RefreshTokenRequest token);
    Task<AuthResponse> GenerateAuthResponseAsync(VigigUser vigigUser)  ;
}