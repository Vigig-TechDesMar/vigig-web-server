﻿using Vigig.Common.Attribute;
using Vigig.Service.Constants;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IGigServiceService
{
    Task<ServiceActionResult> GetAllAsync();
    Task<ServiceActionResult> GetById(Guid gigId);
    Task<ServiceActionResult> AddAsync(GigServiceRequest request);
    Task<ServiceActionResult> UpdateAsync(UpdateGigServiceRequest request);
    Task<ServiceActionResult> DeactivateAsync(Guid gigId);
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    Task<ServiceActionResult> GetACServicesByCategory(BasePaginatedRequest request);
    
    Task<ServiceActionResult> GetACServiceByCategory(Guid id, BasePaginatedRequest request);

    Task<ServiceActionResult> SearchGigService(SearchUsingGet request);
}