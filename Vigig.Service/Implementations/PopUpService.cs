using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Event;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Implementations;

public class PopUpService : IPopUpService
{
    private readonly IPopUpRepository _popUpRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PopUpService(IPopUpRepository popUpRepository, IEventRepository eventRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _popUpRepository = popUpRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var popUps = _mapper.ProjectTo<DtoPopUp>(await _popUpRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = popUps
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var popUp = (await _popUpRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault()
                     ?? throw new PopUpNotFoundException(id.ToString(),nameof(PopUp.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoPopUp>(popUp)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var popUps = _mapper.ProjectTo<DtoPopUp>(
            await _popUpRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoPopUp>(popUps, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchPopUp(SearchUsingGet request)
    {
        var popUps = (await _popUpRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoPopUp>>(SearchHelper.BuildSearchResult<PopUp>(popUps, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreatePopUpRequest request)
    {
        //Check Event
        if (!await _eventRepository.ExistsAsync(sc => sc.Id == request.EventId && sc.IsActive))
            throw new EventNotFoundException(request.EventId,nameof(Event.Id));

        var popUp = _mapper.Map<PopUp>(request);
        await _popUpRepository.AddAsync(popUp);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoPopUp>(popUp),
            StatusCode = StatusCodes.Status201Created
        };
    }
    
    public async Task<ServiceActionResult> UpdateAsync(UpdatePopUpRequest request)
    {
        //Check Event
        if (!await _eventRepository.ExistsAsync(sc => sc.Id == request.EventId && sc.IsActive))
            throw new EventNotFoundException(request.EventId,nameof(Event.Id));

        var popUp = (await _popUpRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault()
                     ?? throw new PopUpNotFoundException(request.Id,nameof(PopUp.Id));

        _mapper.Map(request, popUp);
        await _popUpRepository.UpdateAsync(popUp);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoPopUp>(popUp),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}