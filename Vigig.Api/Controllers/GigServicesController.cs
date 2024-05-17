using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Api.Controllers;
[Route("/api/[controller]")]
public class GigServicesController : BaseApiController
{
    private readonly IGigServiceService _gigService;

    public GigServicesController(IGigServiceService gigService)
    {
        _gigService = gigService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllServices()
    {
        return await ExecuteServiceLogic(async () 
            => await _gigService.GetAllAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetServices([FromQuery]BasePaginatedRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _gigService.GetPaginatedResultAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetServiceById(Guid id)
    {
        return await ExecuteServiceLogic(async () 
            => await _gigService.GetById(id).ConfigureAwait(false)).ConfigureAwait(false);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddService(GigServiceRequest request)
    {
        return await ExecuteServiceLogic(async () 
            => await _gigService.AddAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateService([FromBody] UpdateGigServiceRequest request)
    {
        return await ExecuteServiceLogic(async ()
            => await _gigService.UpdateAsync(request).ConfigureAwait(false)).ConfigureAwait(false);
    }
}