using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Common.Attribute;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Authentication;

namespace Vigig.Api.Controllers;
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly IOauthService _oauthService;
    public AuthController(IAuthService authService, IOauthService oauthService)
    {
        _authService = authService;
        _oauthService = oauthService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _authService.RegisterAsync(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _authService.LoginAsync(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenRequest request)
    {
        return await ExecuteServiceLogic(
            async()=> await _authService.RefreshTokenAsync(request).ConfigureAwait(false)
        ).ConfigureAwait(false);
    }

    [HttpPost("google-oauth")]
    public async Task<IActionResult> GoogleLoginAsync(GoogleAuthRequest request)
    {
        return await ExecuteServiceLogic(
            async () => await _oauthService.LoginAsync(request)).ConfigureAwait(false);
    }
}