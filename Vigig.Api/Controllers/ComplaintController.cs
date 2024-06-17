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
public class ComplaintController : BaseApiController
{
    private readonly IComplaintService _complaintService;

    public ComplaintController(IComplaintService complaintService)
    {
        _complaintService = complaintService;
    }

    [HttpGet("all")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetAllComplaints()
    {
        return await ExecuteServiceLogic(async () =>
            await _complaintService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> GetComplaintById(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _complaintService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchUsingGet(SearchUsingGet request)
    {
        return await ExecuteServiceLogic(async () => 
            await _complaintService.SearchComplaint(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddComplaint(CreateComplaintRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _complaintService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateServiceCategory([FromBody] UpdateComplaintRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _complaintService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _complaintService.DeleteAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
}