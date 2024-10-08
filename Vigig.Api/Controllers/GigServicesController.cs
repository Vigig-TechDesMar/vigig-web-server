﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.DAL.Interfaces;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GigServicesController : BaseApiController
{
    private readonly IGigServiceService _gigService;
    private readonly IProviderServiceService _providerServiceService;

    public GigServicesController(IGigServiceService gigService, IProviderServiceService providerServiceService)
    {
        _gigService = gigService;
        _providerServiceService = providerServiceService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllServices()
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetServices([FromQuery] BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceById(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddService([FromBody] GigServiceRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.AddAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateService([FromBody] UpdateGigServiceRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteService(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.DeactivateAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet("/ac-services")]
    [AllowAnonymous]
    public async Task<IActionResult> GetACServices([FromQuery] BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.GetACServicesByCategory(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet("/ac-services/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetACServices([FromBody] Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchUsingGet([FromQuery]SearchUsingGet request)
    {
        return await ExecuteServiceLogic(async() =>
            await _gigService.SearchGigService(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
}