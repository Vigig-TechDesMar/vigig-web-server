using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IEventImageService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetCurrentPopUpAsync();
    
    Task<ServiceActionResult> GetCurrentBannerAsync();
    
    Task<ServiceActionResult> SearchEventImage(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateEventImageRequest request);
    
    Task<ServiceActionResult> DeleteAsync(Guid id);
}