using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IEventService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);

    Task<ServiceActionResult> AddAsync(CreateEventRequest request);
    
    Task<ServiceActionResult> SearchEvent(SearchUsingGet request);

    Task<ServiceActionResult> UpdateAsync(UpdateEventRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
    
}