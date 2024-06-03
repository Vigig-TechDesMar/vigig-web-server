using AutoMapper;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Event;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Implementations;

public class EventImageService : IEventImageService
{
    private readonly IEventImageRepository _eventImageRepository;
    private readonly IBannerRepository _bannerRepository;
    private readonly IPopUpRepository _popUpRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public EventImageService(IEventImageRepository eventImageRepository, IBannerRepository bannerRepository, IPopUpRepository popUpRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _eventImageRepository = eventImageRepository;
        _bannerRepository = bannerRepository;
        _popUpRepository = popUpRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var images = _mapper.ProjectTo<DtoEventImage>(await _eventImageRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = images
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var image = (await _eventImageRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault()
                    ?? throw new EventImageNotFoundException(id.ToString());
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoEventImage>(image)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var eventImages = _mapper.ProjectTo<DtoEventImage>(
            await _eventImageRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoEventImage>(eventImages, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchEventImage(SearchUsingGet request)
    {
        var images = (await _eventImageRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoEventImage>>(SearchHelper.BuildSearchResult<EventImage>(images, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateEventImageRequest request)
    {
        //Check Banner

        //Check PopUp
        
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateEventImageRequest request)
    {
        //Check Banner

        //Check PopUp
        
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}