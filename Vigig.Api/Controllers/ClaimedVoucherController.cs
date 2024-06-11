using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Voucher;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ClaimedVoucherController : BaseApiController
{
    
    private readonly IClaimedVoucherService _claimedVoucherService;

    public ClaimedVoucherController(IClaimedVoucherService claimedVoucherService)
    {
        _claimedVoucherService = claimedVoucherService;
    }

    [HttpGet("all")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetAllClaimedVouchers()
    {
        return await ExecuteServiceLogic(async () =>
            await _claimedVoucherService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetClaimedVouchers([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _claimedVoucherService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchUsingGet(SearchUsingGet request)
    {
        return await ExecuteServiceLogic(async () => 
            await _claimedVoucherService.SearchClaimedVoucher(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddClaimedVoucher(CreateClaimedVoucherRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _claimedVoucherService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateServiceCategory([FromBody] UpdateClaimedVoucherRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _claimedVoucherService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _claimedVoucherService.DeleteAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
}