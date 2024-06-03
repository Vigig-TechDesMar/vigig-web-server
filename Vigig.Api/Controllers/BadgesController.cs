using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Badge;

namespace Vigig.Api.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BadgesController : BaseApiController
{
    private readonly IBadgeService _badgeService;

    public BadgesController(IBadgeService badgeService)
    {
        _badgeService = badgeService;
    }

    [HttpGet("/all")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetAllBadges()
    {
        return await ExecuteServiceLogic(async () => 
            await _badgeService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBadges([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _badgeService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBadgeById(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _badgeService.GetByIdAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddBadge(CreateBadgeRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _badgeService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateServiceCategory([FromBody] UpdateBadgeRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _badgeService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _badgeService.DeactivateAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }

}