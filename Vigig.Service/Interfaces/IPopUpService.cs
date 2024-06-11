using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IPopUpService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchPopUp(SearchUsingGet request);

    Task<ServiceActionResult> GetActivePopUp();

    Task<ServiceActionResult> AddAsync(CreatePopUpRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdatePopUpRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}