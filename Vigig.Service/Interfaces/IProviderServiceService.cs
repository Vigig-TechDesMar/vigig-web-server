using Microsoft.AspNetCore.Mvc;
using Vigig.Common.Attribute;
using Vigig.Domain.Entities;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IProviderServiceService
{
    Task<ServiceActionResult> GetAirConditionerServicesByTypeAsync(string type, BasePaginatedRequest request);

    Task<ServiceActionResult> GetProviderServiceByIdAsync(Guid id);
    
    Task<ProviderService> RetrieveProviderServiceByIdAsync(Guid id);

    Task<ServiceActionResult> GetOwnProviderServiceAsync(string token);

    Task<ServiceActionResult> SearchProviderServiceAsync(SearchUsingGet request);

    Task<ServiceActionResult> UpdateProviderService(string token, Guid id, CreateProviderServiceRequest request);
} 