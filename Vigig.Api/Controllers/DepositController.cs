using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DepositController : BaseApiController
{
    
    private readonly IDepositService _depositService;
    private string GetJwtToken()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
        var token = authorizationHeader.Replace("bearer", "", StringComparison.OrdinalIgnoreCase).Trim();
        return token;
    }

    public DepositController(IDepositService depositService)
    {
        _depositService = depositService;
    }

    [HttpGet("all")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetAllDeposits()
    {
        return await ExecuteServiceLogic(async () =>
            await _depositService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetDeposits([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _depositService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDepositById(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _depositService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchUsingGet(SearchUsingGet request)
    {
        return await ExecuteServiceLogic(async () => 
            await _depositService.SearchDeposit(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.NonClient)]
    public async Task<IActionResult> AddDeposit(CreateDepositRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _depositService.AddAsync(request,GetJwtToken())).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateServiceCategory([FromBody] UpdateDepositRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _depositService.UpdateAsync(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _depositService.DeleteAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
}