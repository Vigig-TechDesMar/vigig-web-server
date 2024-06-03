using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.DAL.Implementations;
using Vigig.Service.Constants;
using Vigig.Service.Implementations;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.SubscriptionPlan;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SubscriptionPlanController: BaseApiController
{
    private readonly ISubscriptionPlanService _subscriptionPlanService;

    public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
    {
        _subscriptionPlanService = subscriptionPlanService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPlans()
    {
        return await ExecuteServiceLogic(async () =>
            await _subscriptionPlanService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlans([FromQuery] BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _subscriptionPlanService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlanById(Guid Id)
    {
        return await ExecuteServiceLogic(async () =>
            await _subscriptionPlanService.GetById(Id).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddPlan([FromBody] CreateSubscriptionPlanRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _subscriptionPlanService.AddAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdatePlan([FromBody] UpdateSubscriptionPlanRequest request)
    {
        return await ExecuteServiceLogic(async () =>
            await _subscriptionPlanService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeletePlan(Guid id)
    {
        return await ExecuteServiceLogic(async () =>
            await _subscriptionPlanService.DeactivateAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
}