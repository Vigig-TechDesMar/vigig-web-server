using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProviderKPIController : BaseApiController
{
    private readonly IProviderKPIService _providerKPIService;

    public ProviderKPIController(IProviderKPIService providerKPIService)
    {
        _providerKPIService = providerKPIService;
    }

    [HttpGet("all")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetAllProviderKPIs()
    {
        return await ExecuteServiceLogic(async () =>
            await _providerKPIService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProviderKPIs([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _providerKPIService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProviderKPIById(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _providerKPIService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchUsingGet(SearchUsingGet request)
    {
        return await ExecuteServiceLogic(async () => 
            await _providerKPIService.SearchProviderKPI(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddProviderKPI(CreateProviderKPIRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _providerKPIService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateServiceCategory([FromBody] UpdateProviderKPIRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _providerKPIService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _providerKPIService.DeleteAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
}