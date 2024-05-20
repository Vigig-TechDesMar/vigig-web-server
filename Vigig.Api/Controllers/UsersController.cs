using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Api.Controllers;
[Route("/api/[controller]")]
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
    public async Task<IActionResult> UploadService(CreateProviderServiceRequest request)
    {
        throw new InsufficientMemoryException();
    }
}