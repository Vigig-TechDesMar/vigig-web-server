using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Fees;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Service.Implementations;

public class SubscriptionFeeService : ISubscriptionFeeService
{
    private readonly ISubscriptionFeeRepository _subscriptionFeeRepository;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubscriptionFeeService(ISubscriptionFeeRepository subscriptionFeeRepository, ISubscriptionPlanRepository subscriptionPlanRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _subscriptionFeeRepository = subscriptionFeeRepository;
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
        
    public async Task<ServiceActionResult> GetAllAsync()
    {
        var subscriptionFees = _mapper.ProjectTo<DtoSubscriptionFee>(await _subscriptionFeeRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = subscriptionFees
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var subscriptionFee = (await _subscriptionFeeRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault() ??
                              throw new SubscriptionFeeNotFoundException(id.ToString(),nameof(SubscriptionFee.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoSubscriptionFee>(subscriptionFee)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var subscriptionFees = _mapper.ProjectTo<DtoSubscriptionFee>(
            await _subscriptionPlanRepository.FindAsync(s => s.IsActive));
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoSubscriptionFee>(subscriptionFees, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchSubscriptionFee(SearchUsingGet request)
    {
        var subscriptionFees = (await _subscriptionFeeRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoSubscriptionFee>>(SearchHelper.BuildSearchResult<SubscriptionFee>(subscriptionFees, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateSubscriptionFeeRequest request)
    {
        if (!await _subscriptionPlanRepository.ExistsAsync(sc => sc.Id == request.SubscriptionPlanId && sc.IsActive))
            throw new SubscriptionPlanNotFoundException(request.SubscriptionPlanId,nameof(SubscriptionFee.Id));
        
        //Validation
        if (request.CreatedDate is null)
            request.CreatedDate = DateTime.Now;
        
        var subscriptionFee = _mapper.Map<SubscriptionFee>(request);
        await _subscriptionFeeRepository.AddAsync(subscriptionFee);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoSubscriptionFee>(subscriptionFee),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateSubscriptionFeeRequest request)
    {
        if (!await _subscriptionPlanRepository.ExistsAsync(sc => sc.Id == request.SubscriptionPlanId && sc.IsActive))
            throw new SubscriptionPlanNotFoundException(request.SubscriptionPlanId,nameof(SubscriptionPlan.Id));

        var subscriptionFee =
            (await _subscriptionFeeRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault() ??
            throw new SubscriptionFeeNotFoundException(request.Id,nameof(SubscriptionFee.Id));

        _mapper.Map(request, subscriptionFee);
        await _subscriptionFeeRepository.UpdateAsync(subscriptionFee);
        await _unitOfWork.CommitAsync();
        
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoSubscriptionFee>(subscriptionFee),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}