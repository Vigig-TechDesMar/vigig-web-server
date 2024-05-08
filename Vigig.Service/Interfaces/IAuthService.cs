using Vigig.Common.Interfaces;
using Vigig.Domain.Models;
using Vigig.Service.Models;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.Interfaces;

public interface IAuthService : IAutoRegisterable
{
    Task<ServiceActionResult> RegisterAsync(RegisterRequest request);
    Task<ServiceActionResult> LoginAsync(LoginRequest request);
    Task<ServiceActionResult> RefreshTokenAsync(RefreshTokenRequest token);

    Task<AuthResponse> GenerateAuthResponseAsync(Customer customer)  ;
}