using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Api.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    private string GetJwtToken()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
         var token = authorizationHeader.Replace("bearer", "", StringComparison.OrdinalIgnoreCase).Trim();
         return token;
    }
    
    [HttpGet("/user-profile")]
    public async Task<IActionResult> GetProfile()
    {
        return await ExecuteServiceLogic(async () =>
            await _userService.GetProfileInformation(GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPost("/register-gigservice")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> UploadService([FromForm]CreateProviderServiceRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _userService.UploadService(GetJwtToken(),request).ConfigureAwait(false)).ConfigureAwait(false);
    }
}