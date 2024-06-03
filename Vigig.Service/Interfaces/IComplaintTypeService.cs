using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Complaint;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IComplaintTypeService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchComplaintType(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateComplaintTypeRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateComplaintTypeRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}