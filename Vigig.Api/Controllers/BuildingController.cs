using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Domain.Models;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Building;

namespace Vigig.Api.Controllers;
[Route("/api/[controller]")]
public class BuildingController : BaseApiController
{
    private readonly IBuildingService _buildingService;

    public BuildingController(IBuildingService buildingService)
    {
        _buildingService = buildingService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBuilding(CreateBuildingRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _buildingService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBuilding()
    {
        return await ExecuteServiceLogic(async ()
        => await _buildingService.GetAllAsync()).ConfigureAwait(false);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBuildingById(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _buildingService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBuilding([FromBody]UpdateBuildingRequest building)
    {
        return await ExecuteServiceLogic(async ()
            => await _buildingService.UpdateAsync(building).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBuilding(Guid buildingId)
    {
        return await ExecuteServiceLogic(async ()
            => await _buildingService.DeactivateAsync(buildingId).ConfigureAwait(false)).ConfigureAwait(false);
    }
}