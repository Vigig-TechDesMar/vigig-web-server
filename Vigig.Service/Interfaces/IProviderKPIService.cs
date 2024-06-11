using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IProviderKPIService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchProviderKPI(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateProviderKPIRequest request, string token);

    Task<ServiceActionResult> UpdateAsync(UpdateProviderKPIRequest request, string token);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}