using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Common.Models;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Building;

namespace Vigig.Api.Controllers;
[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BuildingsController : BaseApiController
{
    private readonly IBuildingService _buildingService;
    public BuildingsController(IBuildingService buildingService)
    {
        _buildingService = buildingService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBuilding()
    {
        return await ExecuteServiceLogic(async ()
        => await _buildingService.GetAllAsync()).ConfigureAwait(false);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBuilding([FromQuery] BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _buildingService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBuildingById(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _buildingService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddBuilding(CreateBuildingRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _buildingService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateBuilding([FromBody]UpdateBuildingRequest building)
    {
        return await ExecuteServiceLogic(async ()
            => await _buildingService.UpdateAsync(building).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteBuilding(Guid buildingId)
    {
        return await ExecuteServiceLogic(async ()
            => await _buildingService.DeactivateAsync(buildingId).ConfigureAwait(false)).ConfigureAwait(false);
    }
}