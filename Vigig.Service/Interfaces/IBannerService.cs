using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IBannerService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchBanner(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateBannerRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateBannerRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}