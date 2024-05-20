using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Badge;

namespace Vigig.Api.Controllers;
[Route("/api/[controller]")]
public class BadgeController : BaseApiController
{
    private readonly IBadgeService _badgeService;

    public BadgeController(IBadgeService badgeService)
    {
        _badgeService = badgeService;
    }

    [HttpGet("/all")]
    public async Task<IActionResult> GetAllBadges()
    {
        return await ExecuteServiceLogic(async () => 
            await _badgeService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpGet]
    public async Task<IActionResult> GetBadges([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _badgeService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBadgeById(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _badgeService.GetByIdAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBadge(CreateBadgeRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _badgeService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateServiceCategory([FromBody] UpdateBadgeRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _badgeService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _badgeService.DeactivateAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }

}