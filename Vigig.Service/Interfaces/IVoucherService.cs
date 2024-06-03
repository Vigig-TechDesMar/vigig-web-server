using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Voucher;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IVoucherService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchVoucher(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateVoucherRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateVoucherRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}