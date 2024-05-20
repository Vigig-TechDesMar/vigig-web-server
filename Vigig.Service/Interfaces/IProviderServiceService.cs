using Microsoft.AspNetCore.Mvc;
using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IProviderServiceService
{
    Task<ServiceActionResult> GetAirConditionerServicesByTypeAsync(string type, BasePaginatedRequest request);

    Task<ServiceActionResult> GetProviderServiceByIdAsync(Guid id);
}