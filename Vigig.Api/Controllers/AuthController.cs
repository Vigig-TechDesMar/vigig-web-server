using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Authentication;

namespace Vigig.Api.Controllers;
[Route("/api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _authService.RegisterAsync(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }


}