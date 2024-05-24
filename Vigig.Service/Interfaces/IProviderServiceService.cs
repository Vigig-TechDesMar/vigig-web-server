using Microsoft.AspNetCore.Mvc;
using Vigig.Common.Attribute;
using Vigig.Domain.Entities;
using Vigig.Service.Models.Common;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IProviderServiceService
{
    Task<ServiceActionResult> GetAirConditionerServicesByTypeAsync(string type, BasePaginatedRequest request);

    Task<ServiceActionResult> GetProviderServiceByIdAsync(Guid id);
    
    Task<ProviderService> RetrieveProviderServiceByIdAsync(Guid id);
}