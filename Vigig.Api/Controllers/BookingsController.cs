using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Booking;

namespace Vigig.Api.Controllers;
[Route("/api/[controller]")]
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
    
    [HttpPut("/bookings/{id:guid}/accepted")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> AcceptBooking(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.AcceptBookingAsync(id,GetJwtToken())).ConfigureAwait(false);
    }
    
    [HttpPut("/bookings/{id:guid}/declined")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> DeclineBooking(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.DeclineBookingAsync(id,GetJwtToken())).ConfigureAwait(false);
    }
    
    [HttpPut("/bookings/{id:guid}/canceled-by-client")]
    [Authorize(Roles = UserRoleConstant.Client)]
    public async Task<IActionResult> CancelBookingByClient(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.CancelBookingByClientAsync(id,GetJwtToken())).ConfigureAwait(false);
    }
    
    [HttpPut("/bookings/{id:guid}/canceled-by-provider")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> CancelBookingByProvider(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.CancelBookingByProviderAsync(id,GetJwtToken())).ConfigureAwait(false);
    }
    
    [HttpPut("/bookings/{id:guid}/completed")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> CompleteBooking(Guid id, BookingCompleteRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _bookingService.CompleteBookingAsync(id, request,GetJwtToken())).ConfigureAwait(false);
    }

    [HttpGet("own-booking")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOwnBooking()
    {
        return await ExecuteServiceLogic(async () => 
            await _bookingService.LoadOwnBookingAsync(GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }



}