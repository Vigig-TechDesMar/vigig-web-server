using Vigig.Common.Interfaces;
using Vigig.Domain.Models;
using Vigig.Service.Models;
using Vigig.Service.Models.Request.Building;

namespace Vigig.Service.Interfaces;

public interface IBuildingService : IAutoRegisterable
{
    Task<ServiceActionResult> AddAsync(CreateBuildingRequest request);
    Task<ServiceActionResult> GetAllAsync();
    Task<ServiceActionResult> GetById(Guid buildingId);
    Task<ServiceActionResult> UpdateAsync(UpdateBuildingRequest request);
    Task<ServiceActionResult> DeactivateAsync(Guid buildingId);

}