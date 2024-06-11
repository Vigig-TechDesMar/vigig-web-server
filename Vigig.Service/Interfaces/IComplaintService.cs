using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Complaint;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IComplaintService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> SearchComplaint(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateComplaintRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateComplaintRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}