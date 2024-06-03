using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Voucher;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IClaimedVoucherService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    Task<ServiceActionResult> SearchClaimedVoucher(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateClaimedVoucherRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateClaimedVoucherRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}