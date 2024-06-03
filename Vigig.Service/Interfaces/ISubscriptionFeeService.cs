using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;
using Vigig.Service.Models.Request.SubscriptionPlan;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface ISubscriptionFeeService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    Task<ServiceActionResult> SearchSubscriptionFee(SearchUsingGet request);

    //Admin
    Task<ServiceActionResult> AddAsync(CreateSubscriptionFeeRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateSubscriptionFeeRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);

}