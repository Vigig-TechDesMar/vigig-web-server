using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Event;
using Vigig.Domain.Entities;
using Vigig.Service.Enums;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Implementations;

public class LeaderBoardService : ILeaderBoardService
{
    private readonly ILeaderBoardRepository _leaderBoardRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IVigigUserRepository _vigigUserRepository;

    public LeaderBoardService(ILeaderBoardRepository leaderBoardRepository, IEventRepository eventRepository, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IVigigUserRepository vigigUserRepository)
    {
        _leaderBoardRepository = leaderBoardRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
        _vigigUserRepository = vigigUserRepository;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var leaderBoards = _mapper.ProjectTo<DtoLeaderBoard>(await _leaderBoardRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = leaderBoards
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var leaderBoard = (await _leaderBoardRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault()
                          ?? throw new LeaderBoardNotFoundException(id.ToString(),nameof(LeaderBoard.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoLeaderBoard>(leaderBoard)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var leaderboards = _mapper.ProjectTo<DtoLeaderBoard>(
            await _leaderBoardRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoLeaderBoard>(leaderboards, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchLeaderBoard(SearchUsingGet request)
    {
        var leaderBoards = (await _leaderBoardRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoLeaderBoard>>(
                SearchHelper.BuildSearchResult<LeaderBoard>(leaderBoards, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateLeaderBoardRequest request)
    {
        //Check Event
        if (!await _eventRepository.ExistsAsync(sc => sc.Id == request.EventId && sc.IsActive))
            throw new EventNotFoundException(request.EventId,nameof(Event.Id));

        var leaderboard = _mapper.Map<LeaderBoard>(request);
        await _leaderBoardRepository.AddAsync(leaderboard);
        await _unitOfWork.CommitAsync();

        if (request.EmailUser)
            await EmailProviderAboutNewLeaderBoard(request,leaderboard);
        
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoLeaderBoard>(leaderboard),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateLeaderBoardRequest request)
    {
        
        //Check Event
        if (!await _eventRepository.ExistsAsync(sc => sc.Id == request.EventId && sc.IsActive))
            throw new EventNotFoundException(request.EventId,nameof(Event.Id));

        var leaderBoard = (await _leaderBoardRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault()
                          ?? throw new LeaderBoardNotFoundException(request.Id,nameof(LeaderBoard.Id));

        _mapper.Map(request, leaderBoard);
        await _leaderBoardRepository.UpdateAsync(leaderBoard);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoLeaderBoard>(leaderBoard),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
    private async Task EmailProviderAboutNewLeaderBoard(CreateLeaderBoardRequest request, LeaderBoard leaderBoard)
    {
        var users = (await _vigigUserRepository.FindAsync(sc =>
            sc.IsActive && sc.Roles.Contains<>(UserRole.Provider))).ToList();
        await _emailService.SendEmailToUsersAsync(users, leaderBoard.Name,
            leaderBoard.Description + request.Body);
    }
}