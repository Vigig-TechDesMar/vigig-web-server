using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Service;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Implementations;

public class ProviderServiceService : IProviderServiceService
{
    private readonly IProviderServiceRepository _providerServiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaService _mediaService;
    private readonly IGigServiceRepository _gigServiceRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public ProviderServiceService(IProviderServiceRepository providerServiceRepository, IGigServiceRepository gigServiceRepository, IMapper mapper, IJwtService jwtService, IMediaService mediaService, IUnitOfWork unitOfWork)
    {
        _providerServiceRepository = providerServiceRepository;
        _gigServiceRepository = gigServiceRepository;
        _mapper = mapper;
        _jwtService = jwtService;
        _mediaService = mediaService;
        _unitOfWork = unitOfWork;
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
//provider.UserId = {Guid} d126ffae-7d5a-4aed-02eb-08dc789bd93f 

    public async Task<ServiceActionResult> SearchProviderServiceAsync(SearchUsingGet request)
    {
        var providerService = (await _providerServiceRepository.GetAllAsync())
            .Include(x => x.Provider)
            .Include(x => x.Service)
            .Include(x => x.ServiceImages)
            .AsEnumerable();
        var searchResults = _mapper.Map<IEnumerable<DtoProviderService>>(SearchHelper.BuildSearchResult<ProviderService>(providerService, request));
        var paginatedResult = PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> UpdateProviderService(string token, Guid id, CreateProviderServiceRequest request)
    {
        var gigService = await _gigServiceRepository.GetAsync(x => x.Id == request.ServiceId) 
                         ?? throw new GigServiceNotFoundException(request.ServiceId, nameof(GigService.Id));
        var service = await GetProviderServiceAsync(token, id);
        service.ServiceId = gigService.Id;
        service.StickerPrice = request.StickerPrice ;
        service.Description = request.Description ?? service.Description;
        var newImageUrls = (await _mediaService.GetUrlAfterUploadingFile(request.Images));
        var serviceImages = newImageUrls.Select(x => new ServiceImage
        {
            ImageUrl = x,
        }).ToList();
        service.ServiceImages = serviceImages;
        await _providerServiceRepository.UpdateAsync(service);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }
    

    private async Task<ProviderService> GetProviderServiceAsync(string token, Guid id)
    {
        var authModel = _jwtService.GetAuthModel(token);
        var service =
            (await _providerServiceRepository.FindAsync(x => x.Id == id && x.ProviderId == authModel.UserId && x.IsActive))
            .FirstOrDefault() ?? throw new Exception($"provider do not have provider service {id}");
        return service;
    }
}