using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Badge;
using Vigig.Service.Models.Request.Building;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IBadgeService
{
    Task<ServiceActionResult> AddAsync(CreateBadgeRequest request);
    Task<ServiceActionResult> GetAllAsync();
    Task<ServiceActionResult> GetByIdAsync(Guid id);
    Task<ServiceActionResult> UpdateAsync(UpdateBadgeRequest request);
    Task<ServiceActionResult> DeactivateAsync(Guid id);
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
}