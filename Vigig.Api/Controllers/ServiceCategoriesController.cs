using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.DAL.Interfaces;
using Vigig.Service.Constants;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.GigService;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Api.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ServiceCategoriesController : BaseApiController
{
    private readonly IServiceCategoryService _gigCategoryServiceService;

    public ServiceCategoriesController(IGigServiceRepository gigService, IServiceCategoryService gigCategoryServiceService)
    {
        _gigCategoryServiceService = gigCategoryServiceService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllServiceCategories()
    {
        return await ExecuteServiceLogic(async () => 
            await _gigCategoryServiceService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpGet]
    public async Task<IActionResult> GetServiceCategories([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () => 
            await _gigCategoryServiceService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetServiceCategoryById(Guid id)
    {
        return await ExecuteServiceLogic(async () => 
            await _gigCategoryServiceService.GetByIdAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> AddServiceCategory([FromForm]ServiceCategoryRequest  request)
    {
        return await ExecuteServiceLogic(async () 
            => await _gigCategoryServiceService.AddAsync(request)).ConfigureAwait(false);
    }

    [HttpPut]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> UpdateServiceCategory([FromForm] UpdateServiceCategoryRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigCategoryServiceService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }

    [HttpDelete]
    [Authorize(Roles = UserRoleConstant.InternalUser)]
    public async Task<IActionResult> DeleteServiceCategory(Guid id)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigCategoryServiceService.DeactivateAsync(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
   
}