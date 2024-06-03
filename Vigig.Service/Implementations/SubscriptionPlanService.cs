using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.SubscriptionPlan;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.AlreadyExist;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.SubscriptionPlan;

namespace Vigig.Service.Implementations;

public class SubscriptionPlanService: ISubscriptionPlanService
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubscriptionPlanService(ISubscriptionPlanRepository subscriptionPlanRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var plans = _mapper.ProjectTo<DtoSubscriptionPlan>(await _subscriptionPlanRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = plans
        };
    }

    public async Task<ServiceActionResult> GetById(Guid Id)
    {
        var exist = (await _subscriptionPlanRepository.FindAsync(s => Id == s.Id && s.IsActive))
            .FirstOrDefault();

        if (exist is null)
            throw new SubscriptionPlanNotFoundException(Id);
        var plan = _mapper.Map<DtoSubscriptionPlan>(exist);
        return new ServiceActionResult(true)
        {
            Data = plan
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateSubscriptionPlanRequest request)
    {
        var plan = _mapper.Map<SubscriptionPlan>(request);
        await _subscriptionPlanRepository.AddAsync(plan);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoSubscriptionPlan>(plan),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateSubscriptionPlanRequest request)
    {
        var plan = (await _subscriptionPlanRepository.FindAsync(s => s.Id == request.Id && s.IsActive))
            .FirstOrDefault() ?? throw new SubscriptionPlanNotFoundException(request.Id);
        
        _mapper.Map(request, plan);
        await _subscriptionPlanRepository.UpdateAsync(plan);
        await _unitOfWork.CommitAsync();

        return new ServiceActionResult(true)
        {
            Data = plan,
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public Task<ServiceActionResult> DeactivateAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var plans = _mapper.ProjectTo<DtoSubscriptionPlan>(
            await _subscriptionPlanRepository.FindAsync(s => s.IsActive));
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoSubscriptionPlan>(plans, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }
}