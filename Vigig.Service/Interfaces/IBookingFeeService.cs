using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IBookingFeeService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchBookingFee(SearchUsingGet request);

    //Admin
    Task<ServiceActionResult> AddAsync(CreateBookingFeeRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateBookingFeeRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
    
}