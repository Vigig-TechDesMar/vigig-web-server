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
                    ?? throw new EventImageNotFoundException(id.ToString(),nameof(EventImage.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoEventImage>(image)
        };
    }

    public async Task<ServiceActionResult> GetCurrentPopUpAsync()
    {
        var popUp = (await _popUpRepository.FindAsync(sc => sc.IsActive)).FirstOrDefault() ??
                    throw new CurrentPopUpNotFoundException();

        var popUpImages = await _eventImageRepository.FindAsync(sc => sc.PopUpId == popUp.Id);
        _mapper.Map<IEnumerable<DtoEventImage>>(popUpImages);
        return new ServiceActionResult(true)
        {
            Data = popUpImages
        };
    }

    public async Task<ServiceActionResult> GetCurrentBannerAsync()
    {
        var banner = (await _bannerRepository.FindAsync(sc => sc.IsActive)).FirstOrDefault() ??
                    throw new CurrentBannerNotFoundException();

        var bannerImages = (await _eventImageRepository.FindAsync(sc => sc.BannerId == banner.Id)).AsEnumerable();
        _mapper.Map<IEnumerable<DtoEventImage>>(bannerImages);
        return new ServiceActionResult(true)
        {
            Data = bannerImages
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
        if (request.BannerId != null)
            if(!await _bannerRepository.ExistsAsync(sc => sc.Id == request.BannerId))
                throw new BannerNotFoundException(request.BannerId, nameof(Banner.Id));
                
        //Check PopUp
        if(request.PopUpId != null)
            if (!await _popUpRepository.ExistsAsync(sc => sc.Id == request.PopUpId))
                throw new PopUpNotFoundException(request.PopUpId, nameof(PopUp.Id));

        var eventImage = new EventImage
        {
            ImageUrl = request.ImageUrl,
            BannerId = request.BannerId,
            PopUpId = request.PopUpId
        };
        
        await _eventImageRepository.AddAsync(eventImage);
        await _unitOfWork.CommitAsync();

        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoEventImage>(eventImage)
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        if (!await _eventImageRepository.ExistsAsync(sc => sc.Id == id))
            throw new EventImageNotFoundException(id, nameof(EventImage.Id));

        throw new NotImplementedException();
    }
}