using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Common.Models;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Api.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EventController : BaseApiController
{
    private readonly IEventService _eventService;
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllEvent()
    {
        return await ExecuteServiceLogic(async ()
            => await _eventService.GetAllAsync()).ConfigureAwait(false);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetEvent([FromQuery] BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _eventService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEventById(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _eventService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddEvent(CreateEventRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _eventService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _eventService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteEvent(Guid eventId)
    {
        return await ExecuteServiceLogic(async ()
            => await _eventService.DeleteAsync(eventId).ConfigureAwait(false)).ConfigureAwait(false);
    }
}