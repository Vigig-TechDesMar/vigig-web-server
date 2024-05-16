using Vigig.Common.Attribute;
using Vigig.Service.Models;
using Vigig.Service.Models.Request.GigService;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IServiceCategoryService
{
    Task<ServiceActionResult> GetAllAsync();
    Task<ServiceActionResult> GetAllByIdAsync(Guid serviceCategoryId);
    Task<ServiceActionResult> AddAsync(ServiceCategoryRequest request);
    Task<ServiceActionResult> UpdateAsync(ServiceCategoryRequest request);
    Task<ServiceActionResult> DeactivateAsync(Guid serviceCategoryId);

}