using Vigig.Common.Attribute;

using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Building;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IBuildingService 
{
    Task<ServiceActionResult> AddAsync(CreateBuildingRequest request);
    Task<ServiceActionResult> GetAllAsync();
    Task<ServiceActionResult> GetById(Guid buildingId);
    Task<ServiceActionResult> UpdateAsync(UpdateBuildingRequest request);
    Task<ServiceActionResult> DeactivateAsync(Guid buildingId);
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);

}