using Vigig.Common.Attribute;
using Vigig.Domain.Entities;
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

    Task<ServiceActionResult> AddAsyncFromBooking(Booking booking, string token);
    
    //Admin
    Task<ServiceActionResult> AddAsync(CreateBookingFeeRequest request, string token);
    
    // Task<ServiceActionResult> UpdateAsync(UpdateBookingFeeRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
    
}