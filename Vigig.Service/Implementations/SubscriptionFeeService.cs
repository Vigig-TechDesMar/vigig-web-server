using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLog.Fluent;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Fees;
using Vigig.Domain.Entities;
using Vigig.Domain.Enums;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Service.Implementations;

public class SubscriptionFeeService : ISubscriptionFeeService
{
    private readonly ISubscriptionFeeRepository _subscriptionFeeRepository;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IJwtService _jwtService;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly ITransactionService _transactionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubscriptionFeeService(ISubscriptionFeeRepository subscriptionFeeRepository, ISubscriptionPlanRepository subscriptionPlanRepository, IUnitOfWork unitOfWork,
        IMapper mapper, IJwtService jwtService, IVigigUserRepository vigigUserRepository, ITransactionService transactionService)
    {
        _subscriptionFeeRepository = subscriptionFeeRepository;
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtService = jwtService;
        _vigigUserRepository = vigigUserRepository;
        _transactionService = transactionService;
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

    public async Task<ServiceActionResult> AddAsync(CreateSubscriptionFeeRequest request, string token)
    {
        // if (!await _subscriptionPlanRepository.ExistsAsync(sc => sc.Id == request.SubscriptionPlanId && sc.IsActive))
        //     throw new SubscriptionPlanNotFoundException(request.SubscriptionPlanId,nameof(SubscriptionFee.Id));
        
        var subscriptionPlan = (await _subscriptionPlanRepository.FindAsync(sc => sc.Id == request.SubscriptionPlanId && sc.IsActive))
            .FirstOrDefault()  ?? throw new SubscriptionPlanNotFoundException(request.SubscriptionPlanId,nameof(SubscriptionFee.Id));
        
        //Validate provider
        var userId = _jwtService.GetSubjectClaim(token);
        var provider = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == (string)userId))
            .Include(x => x.Wallets)
            .FirstOrDefault() ?? throw new UserNotFoundException(userId,nameof(VigigUser.Id));

        provider.PlanExpirationDate = DateTime.Now.AddHours(subscriptionPlan.DurationType);
        await _vigigUserRepository.UpdateAsync(provider);
        
        if (_jwtService.GetAuthModel(token).Role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!");
        
        //Get the wallet
        var wallet = provider.Wallets.FirstOrDefault() ?? throw new WalletNotFoundException(userId, nameof(userId));
        var subscriptionFee = _mapper.Map<SubscriptionFee>(request);
        
        var price = subscriptionPlan.Price ?? 100000;
        subscriptionFee.Status = CashStatus.Pending;
        subscriptionFee.Amount = Math.Round(price/1000);
        subscriptionFee.CreatedDate = DateTime.Now;
        subscriptionFee.ProviderId = Guid.Parse(userId);
        
        await _subscriptionFeeRepository.AddAsync(subscriptionFee);
        
        // Process the transaction
        await _transactionService.ProcessTransactionAsync(subscriptionFee, wallet);

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