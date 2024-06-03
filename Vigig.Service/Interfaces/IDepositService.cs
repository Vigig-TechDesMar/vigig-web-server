using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IDepositService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchDeposit(SearchUsingGet request);
    
    Task<ServiceActionResult> AddAsync(CreateDepositRequest request);

    //Admin
    Task<ServiceActionResult> UpdateAsync(UpdateDepositRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
    
}