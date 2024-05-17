using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.GigService;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IServiceCategoryService
{
    Task<ServiceActionResult> GetAllAsync();
    Task<ServiceActionResult> GetByIdAsync(Guid serviceCategoryId);
    Task<ServiceActionResult> AddAsync(ServiceCategoryRequest request);
    Task<ServiceActionResult> UpdateAsync(UpdateServiceCategoryRequest request);
    Task<ServiceActionResult> DeactivateAsync(Guid serviceCategoryId);
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);

}