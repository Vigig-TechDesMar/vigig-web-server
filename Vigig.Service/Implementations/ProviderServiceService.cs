using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Service;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;

namespace Vigig.Service.Implementations;

public class ProviderServiceService : IProviderServiceService
{
    private readonly IProviderServiceRepository _providerServiceRepository;
    private readonly IGigServiceRepository _gigServiceRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public ProviderServiceService(IProviderServiceRepository providerServiceRepository, IGigServiceRepository gigServiceRepository, IMapper mapper, IJwtService jwtService)
    {
        _providerServiceRepository = providerServiceRepository;
        _gigServiceRepository = gigServiceRepository;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task<ServiceActionResult> GetAirConditionerServicesByTypeAsync(string type, BasePaginatedRequest request)
    {
        var gigService = (await _gigServiceRepository.FindAsync(s => s.ServiceName.Equals(type) && s.IsActive))
            .FirstOrDefault() ?? throw new GigServiceNotFoundException(type,nameof(GigService.ServiceName));
        var typedServices = _mapper.ProjectTo<DtoProviderService>(await _providerServiceRepository.FindAsync(x => x.ServiceId == gigService.Id));
        var paginatedResult = PaginationHelper.BuildPaginatedResult(typedServices, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> GetProviderServiceByIdAsync(Guid id)
    {
        var providerService =
            (await _providerServiceRepository.FindAsync(x => x.IsActive && x.Id == id && x.IsVisible))
            .Include(x => x.Provider)
            .Include(x => x.Service)
            .Include(x => x.ServiceImages)
            .FirstOrDefault() ?? throw new ProviderServiceNotFoundException(id,nameof(ProviderService.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoProviderService>(providerService)
        };
    }

    public async Task<ProviderService> RetrieveProviderServiceByIdAsync(Guid id)  
    {
        var providerService =
            (await _providerServiceRepository.FindAsync(x => x.IsActive && x.Id == id && x.IsVisible))
            .Include(x => x.Provider)
            .FirstOrDefault() ?? throw new ProviderServiceNotFoundException(id,nameof(ProviderService.Id));
        return providerService;
    }

    public async Task<ServiceActionResult> GetOwnProviderServiceAsync(string token)
    {
        var provider = _jwtService.GetAuthModel(token);
        var providerServices = await _providerServiceRepository.FindAsync(x => x.ProviderId == provider.UserId && x.IsActive);
        return new ServiceActionResult(true)
        {
            Data = _mapper.ProjectTo<DtoProviderService>(providerServices)
        };
    }
}