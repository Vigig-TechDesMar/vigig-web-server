using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Complaint;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ComplaintTypeController : BaseApiController
{
    private readonly IComplaintTypeService _complaintTypeService;

    public ComplaintTypeController(IComplaintTypeService complaintTypeService)
    {
        _complaintTypeService = complaintTypeService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllComplaintTypes()
    {
        return await ExecuteServiceLogic(async () =>
            await _complaintTypeService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetComplaintTypes([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _complaintTypeService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetComplaintTypeById(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _complaintTypeService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchUsingGet(SearchUsingGet request)
    {
        return await ExecuteServiceLogic(async () => 
            await _complaintTypeService.SearchComplaintType(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddComplaintType(CreateComplaintTypeRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _complaintTypeService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateServiceCategory([FromBody] UpdateComplaintTypeRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _complaintTypeService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _complaintTypeService.DeleteAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
}