using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IEventImageService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchEventImage(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateEventImageRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateEventImageRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}