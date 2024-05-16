using Vigig.Service.Models;
using Vigig.Service.Models.Request.GigService;

namespace Vigig.Service.Interfaces;

public interface IGigServiceService
{
    Task<ServiceActionResult> GetAllAsync();
    Task<ServiceActionResult> GetById(Guid gigId);
    Task<ServiceActionResult> AddAsync(GigServiceRequest request);
    Task<ServiceActionResult> UpdateAsync(GigServiceRequest request);
    Task<ServiceActionResult> DeactivateAsync(Guid gigId);

}