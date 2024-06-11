using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Event;
using Vigig.Domain.Dtos.Fees;
using Vigig.Domain.Entities;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Implementations;

public class ProviderKPIService : IProviderKPIService
{
    private readonly IProviderKPIRepository _providerKpiRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly ILeaderBoardRepository _leaderBoardRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public ProviderKPIService(IVigigUserRepository vigigUserRepository, ILeaderBoardRepository leaderBoardRepository, IUnitOfWork unitOfWork, IMapper mapper, IProviderKPIRepository providerKpiRepository, IJwtService jwtService)
    {
        _vigigUserRepository = vigigUserRepository;
        _leaderBoardRepository = leaderBoardRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _providerKpiRepository = providerKpiRepository;
        _jwtService = jwtService;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var kpis = _mapper.ProjectTo<DtoProviderKPI>(await _providerKpiRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = kpis
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var kpi = (await _providerKpiRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault()
                  ?? throw new ProviderKPINotFoundException(id.ToString(),nameof(ProviderKPI.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoProviderKPI>(kpi)
        };
    }

    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var kpis = _mapper.ProjectTo<DtoProviderKPI>(
            await _providerKpiRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoProviderKPI>(kpis, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchProviderKPI(SearchUsingGet request)
    {
        var kpis = (await _providerKpiRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoProviderKPI>>(SearchHelper.BuildSearchResult<ProviderKPI>(kpis, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(),request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateProviderKPIRequest request, string token)
    {
        //Provider join in a LeaderBoard
        var role = _jwtService.GetAuthModel(token).Role;
        if (role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!");

        var userId = _jwtService.GetSubjectClaim(token).ToString();
        if (role == UserRoleConstant.Provider && request.ProviderId.ToString() != userId)
            throw new UnauthorizedAccessException("Provider with ID " + userId  + " is not allowed to proceed with this action!");
        
        //Check Provider
        if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.ProviderId && sc.IsActive))
            throw new UserNotFoundException(request.ProviderId,nameof(VigigUser.Id));

        //Check LeaderBoard
        if (!await _leaderBoardRepository.ExistsAsync(sc => sc.Id == request.LeaderBoardId))
            throw new LeaderBoardNotFoundException(request.LeaderBoardId,nameof(ProviderKPI.Id));

        var kpi = _mapper.Map<ProviderKPI>(request);
        await _providerKpiRepository.AddAsync(kpi);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoProviderKPI>(kpi),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateProviderKPIRequest request, string token)
    {
        //Admin only
        var role = _jwtService.GetAuthModel(token).Role;
        if (role == UserRoleConstant.InternalUser)
            throw new UnauthorizedAccessException("Users are not allowed!");

        
        //Check Provider
        if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.ProviderId && sc.IsActive))
            throw new UserNotFoundException(request.ProviderId,nameof(VigigUser.Id));

        //Check LeaderBoard
        if (!await _leaderBoardRepository.ExistsAsync(sc => sc.Id == request.LeaderBoardId))
            throw new LeaderBoardNotFoundException(request.LeaderBoardId,nameof(LeaderBoard.Id));

        var kpi = (await _providerKpiRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault()
                  ?? throw new ProviderKPINotFoundException(request.Id,nameof(ProviderKPI.Id));
        
        _mapper.Map(request, kpi);
        await _providerKpiRepository.UpdateAsync(kpi);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoProviderKPI>(kpi),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}