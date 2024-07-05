using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Wallet;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IWalletService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetOwnedWallet(string token);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchWallet(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateWalletRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateWalletRequest request);

    Task<ServiceActionResult> DeactivateAsync(Guid id);
}