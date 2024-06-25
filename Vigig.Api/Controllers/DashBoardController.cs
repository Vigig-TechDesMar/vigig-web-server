using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.DashBoard;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DashBoardController : BaseApiController
{
    private readonly IDashBoardService _dashBoardService;

    public DashBoardController(IDashBoardService dashBoardService)
    {
        _dashBoardService = dashBoardService;
    }
    
    [HttpGet("admin-bookings")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AdminGetBookings([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.AdminGetBookings(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet("admin-complaints")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AdminGetComplaints([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.AdminGetComplaints(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("admin-new-users")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AdminGetNewUsers([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.AdminGetNewUsers(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("admin-revenues")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AdminGetTotalRevenue([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.AdminGetTotalRevenue(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("admin-events-and-children")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AdminGetEventsAndChildren([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.AdminGetEventsAndChildren(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("admin-vouchers")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AdminGetVoucher([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.AdminGetVoucher(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("admin-transactions")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AdminGetTransaction([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.AdminGetTransaction(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("provider-bookings")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> ProviderGetBookings([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.ProviderGetBookings(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("provider-total-cashflow")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> ProviderGetTotalCashFlow([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.ProviderGetTotalCashFlow(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("provider-complaints")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> ProviderGetComplaints([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.ProviderGetComplaints(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("provider-leaderboards-and-kpis")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> ProviderGetLeaderBoard([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.ProviderGetLeaderBoard(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("provider-transactions")]
    [Authorize(Roles = UserRoleConstant.Provider)]
    public async Task<IActionResult> ProviderGetTransaction([FromQuery] DashBoardRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _dashBoardService.ProviderGetTransaction(request, GetJwtToken()).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    private string GetJwtToken()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
        var token = authorizationHeader.Replace("bearer", "", StringComparison.OrdinalIgnoreCase).Trim();
        return token;
    }
}