using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Badge;
using Vigig.Domain.Dtos.Event;
using Vigig.Domain.Dtos.Fees;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Implementations;

public class BannerService : IBannerService
{
    private readonly IBannerRepository _bannerRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BannerService(IBannerRepository bannerRepository, IEventRepository eventRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _bannerRepository = bannerRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var banners = _mapper.ProjectTo<DtoBanner>(await _bannerRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = banners
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var banner = (await _bannerRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault()
                     ?? throw new BannerNotFoundException(id.ToString(),nameof(Banner.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBanner>(banner)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var banners = _mapper.ProjectTo<DtoBanner>(
            await _bannerRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoBanner>(banners, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchBanner(SearchUsingGet request)
    {
        var banners = (await _bannerRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoBanner>>(SearchHelper.BuildSearchResult<Banner>(banners, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateBannerRequest request)
    {
        //Check Event
        if (!await _eventRepository.ExistsAsync(sc => sc.Id == request.EventId && sc.IsActive))
            throw new EventNotFoundException(request.EventId, nameof(Event.Id));

        var banner = _mapper.Map<Banner>(request);
        await _bannerRepository.AddAsync(banner);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBanner>(banner),
            StatusCode = StatusCodes.Status201Created
        };
    }
    
    public async Task<ServiceActionResult> UpdateAsync(UpdateBannerRequest request)
    {
        //Check Event
        if (!await _eventRepository.ExistsAsync(sc => sc.Id == request.EventId && sc.IsActive))
            throw new EventNotFoundException(request.EventId,nameof(Event.Id));

        var banner = (await _bannerRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault()
                     ?? throw new BannerNotFoundException(request.Id,nameof(Banner.Id));

        _mapper.Map(request, banner);
        await _bannerRepository.UpdateAsync(banner);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBanner>(banner),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}