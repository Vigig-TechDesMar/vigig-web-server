using Vigig.Common.Interfaces;
using Vigig.Service.Models;
using Vigig.Service.Models.Request.Authentication;

namespace Vigig.Service.Interfaces;

public interface IAuthService : IAutoRegisterable
{
    Task<ServiceActionResult> RegisterAsync(RegisterRequest request);
}