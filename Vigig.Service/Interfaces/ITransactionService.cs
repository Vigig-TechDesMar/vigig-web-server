using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Wallet;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface ITransactionService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);

    Task<ServiceActionResult> SearchTransaction(SearchUsingGet request);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    //
    // Task<ServiceActionResult> AddAsync(CreateTransactionRequest request);
    //
    // Task<ServiceActionResult> UpdateAsync(UpdateTransactionRequest request);
    //
    // Task<ServiceActionResult> DeleteAsync(Guid id);
    
}