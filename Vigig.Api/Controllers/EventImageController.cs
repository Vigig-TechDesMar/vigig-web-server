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
public class EventImageController : BaseApiController
{
    
    private readonly IEventImageService _eventImageService;

    public EventImageController(IEventImageService eventImageService)
    {
        _eventImageService = eventImageService;
    }

    [HttpGet("all")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetAllEventImages()
    {
        return await ExecuteServiceLogic(async () =>
            await _eventImageService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("currentBanner")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCurrentBannerEventImages()
    {
        return await ExecuteServiceLogic(async () 
            => await _eventImageService.GetCurrentBannerAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("currentPopUp")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCurrentPopUpEventImages()
    {
        return await ExecuteServiceLogic(async () 
            => await _eventImageService.GetCurrentPopUpAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEventImageById(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _eventImageService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchUsingGet(SearchUsingGet request)
    {
        return await ExecuteServiceLogic(async () => 
            await _eventImageService.SearchEventImage(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddEventImage(CreateEventImageRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _eventImageService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _eventImageService.DeleteAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
}