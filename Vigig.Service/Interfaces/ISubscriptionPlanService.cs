using Vigig.Common.Attribute;
using Vigig.Service.Constants;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.SubscriptionPlan;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface ISubscriptionPlanService
{
    Task<ServiceActionResult> GetAllAsync();
    
    Task<ServiceActionResult> GetById(Guid gigId);
    
    Task<ServiceActionResult> AddAsync(CreateSubscriptionPlanRequest request);
    
    Task<ServiceActionResult> UpdateAsync(UpdateSubscriptionPlanRequest request);
    
    Task<ServiceActionResult> DeactivateAsync(Guid gigId);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
}