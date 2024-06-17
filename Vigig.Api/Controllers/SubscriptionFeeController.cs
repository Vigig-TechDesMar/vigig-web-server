using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SubscriptionFeeController : BaseApiController
{
    private readonly ISubscriptionFeeService _subscriptionFeeService;

    public SubscriptionFeeController(ISubscriptionFeeService subscriptionFeeService)
    {
        _subscriptionFeeService = subscriptionFeeService;
    }

    private string GetJwtToken()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
        var token = authorizationHeader.Replace("bearer", "", StringComparison.OrdinalIgnoreCase).Trim();
        return token;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddSubscriptionFTest([FromBody] CreateSubscriptionFeeRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _subscriptionFeeService.AddAsync(request,GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
}