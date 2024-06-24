using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Booking;

namespace Vigig.Api.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookingsController : BaseApiController
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    private string GetJwtToken()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
        var token = authorizationHeader.Replace("bearer", "", StringComparison.OrdinalIgnoreCase).Trim();
        return token;
    }
    
    [HttpPost("/bookings/placing")]
    [Authorize(Roles = UserRoleConstant.Client)]
    public async Task<IActionResult> PlaceBooking([FromBody] BookingPlaceRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.PlaceBookingAsync(GetJwtToken(),request)).ConfigureAwait(false);
    }
    
    [HttpPut]
    [Route("{id:guid}/accepted")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> AcceptBooking(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.AcceptBookingAsync(id,GetJwtToken())).ConfigureAwait(false);
    }
    
    [HttpPut("{id:guid}/declined")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> DeclineBooking(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.DeclineBookingAsync(id,GetJwtToken())).ConfigureAwait(false);
    }
    
    [HttpPut("{id:guid}/cancelled-by-client")]
    [Authorize(Roles = UserRoleConstant.Client)]
    public async Task<IActionResult> CancelBookingByClient(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.CancelBookingByClientAsync(id,GetJwtToken())).ConfigureAwait(false);
    }
    
    [HttpPut("{id:guid}/cancelled-by-provider")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> CancelBookingByProvider(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.CancelBookingByProviderAsync(id,GetJwtToken())).ConfigureAwait(false);
    }
    
    [HttpPut("{id:guid}/completed")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> CompleteBooking(Guid id,[FromBody]BookingCompleteRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.CompleteBookingAsync(id, request,GetJwtToken())).ConfigureAwait(false);
    }

    [HttpGet("own-bookings/chat")]
    public async Task<IActionResult> GetOwnChatBooking()
    {
        return await ExecuteServiceLogic(async () => 
            await _bookingService.LoadOwnChatBookingAsync(GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet("own-bookings/chat/{id:guid}")]
    public async Task<IActionResult> GetOwnChatBookingDetail(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
                await _bookingService.LoadOwnChatBookingDetailAsync(id, GetJwtToken()).ConfigureAwait(false))
            .ConfigureAwait(false);
    }
    [HttpGet("own-bookings")]
    // [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> GetOwnBooking([FromQuery] IReadOnlyCollection<string>? status)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.LoadAllBookingsAsync(GetJwtToken(), status).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPut("{id:guid}/rating")]
    [Authorize(Roles = UserRoleConstant.Client)]
    public async Task<IActionResult> ReviewBooking(Guid id,BookingRatingRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.RatingBookingAsync(GetJwtToken(),id,request)).ConfigureAwait(false);
    }
}