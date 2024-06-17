using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Event;
using Vigig.Domain.Entities;
using Vigig.Service.Constants;
using Vigig.Service.Enums;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Implementations;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IVigigUserRepository _vigigUserRepository;
    
    public EventService(IEventRepository eventRepository, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IVigigUserRepository vigigUserRepository)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
        _vigigUserRepository = vigigUserRepository;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var events = _mapper.ProjectTo<DtoEvent>(await _eventRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = events
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var ev = (await _eventRepository.FindAsync(sc => sc.IsActive && sc.Id == id)).FirstOrDefault()
            ?? throw new EventNotFoundException(id.ToString(),nameof(Event.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoEvent>(ev)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var ev = _mapper.ProjectTo<DtoEvent>(
            await _eventRepository.FindAsync(s => s.IsActive));
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoEvent>(ev, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }
    
    public async Task<ServiceActionResult> SearchEvent(SearchUsingGet request)
    {
        var events = (await _eventRepository.GetAllAsync()).AsEnumerable();
        var searchResults = _mapper.Map<IEnumerable<DtoEvent>>(SearchHelper.BuildSearchResult<Event>(events, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateEventRequest request)
    {
        var ev = _mapper.Map<Event>(request);
        await _eventRepository.AddAsync(ev);
        await _unitOfWork.CommitAsync();

        if (request.EmailUser)
            EmailUserAboutNewEvent(request, ev);
        
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoEvent>(ev),
            StatusCode = StatusCodes.Status201Created
        };
    }
    

    public async Task<ServiceActionResult> UpdateAsync(UpdateEventRequest request)
    {
        var ev = (await _eventRepository.FindAsync(sc => sc.Id == request.Id && sc.IsActive)).FirstOrDefault()
                 ?? throw new EventNotFoundException(request.Id,nameof(Event.Id));
        _mapper.Map(request, ev);
        await _eventRepository.UpdateAsync(ev);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoEvent>(ev),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    private async Task EmailUserAboutNewEvent(CreateEventRequest request, Event evnt)
    {
        if (request.IsForAll)
        {
            await _emailService.SendEmailToUsersAsync((await _vigigUserRepository.GetAllAsync()).ToList(), evnt.EventTitle,
                evnt.EventDescription??EventConstant.DefaultEventDescription);
        }
        else if(request.Targets.Count > 0)
        {
            foreach (var x in request.Targets)
            {
                var users =(await _vigigUserRepository.FindAsync(sc =>
                    sc.IsActive && sc.Roles.Any(y=> y.Name == x))).ToList();
                await _emailService.SendEmailToUsersAsync(users, evnt.EventTitle,
                    evnt.EventDescription + request.Body);
            }
        }
    }
}