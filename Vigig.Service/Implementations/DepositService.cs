using System.Drawing.Printing;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
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

public class DepositService : IDepositService
{
    private readonly IDepositRepository _depositRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IJwtService _jwtService;
    private readonly ITransactionService _transactionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepositService(IDepositRepository depositRepository, IVigigUserRepository vigigUserRepository, IUnitOfWork unitOfWork, IMapper mapper, IJwtService jwtService, ITransactionService transactionService)
    {
        _depositRepository = depositRepository;
        _vigigUserRepository = vigigUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtService = jwtService;
        _transactionService = transactionService;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var deposits = _mapper.ProjectTo<Deposit>(await _depositRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = deposits
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var deposit = (await _depositRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault();
        if (deposit is null)
            throw new DepositNotFoundException(id.ToString(),nameof(Deposit.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoDeposit>(deposit)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var deposits = _mapper.ProjectTo<DtoDeposit>(
            await _depositRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoDeposit>(deposits, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchDeposit(SearchUsingGet request)
    {
        var deposits = (await _depositRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoDeposit>>(SearchHelper.BuildSearchResult<Deposit>(deposits, request));
        var paginatedResults = PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize,request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateDepositRequest request, string token)
    {
        //Validate provider
        var userId = _jwtService.GetSubjectClaim(token).ToString();
        var provider = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == userId))
            .Include(x => x.Wallets)
            .FirstOrDefault() ?? throw new UserNotFoundException(userId,nameof(VigigUser.Id));

        if (_jwtService.GetAuthModel(token).Role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!");

        //Get the wallet
        var wallet = provider.Wallets.FirstOrDefault() ?? throw new WalletNotFoundException(userId, nameof(userId));

        var deposit = _mapper.Map<Deposit>(request); 
            deposit.Status = CashStatus.Pending;
        await _depositRepository.AddAsync(deposit);
        
        // Process the transaction
        try
        {
            await _transactionService.ProcessTransactionAsync(deposit,wallet);
        }
        catch (Exception ex)
        {
            //Log error
            // subscriptionFee.Status = TransactionStatusConstant.Error;
        }
        
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoDeposit>(deposit),
            StatusCode = StatusCodes.Status201Created
        };
    }

    //Admin
    public async Task<ServiceActionResult> UpdateAsync(UpdateDepositRequest request, string token)
    {
        if (_jwtService.GetAuthModel(token).Role != UserRoleConstant.InternalUser)
            throw new UnauthorizedAccessException("Users are not allowed!");

        if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.ProviderId && sc.IsActive))
            throw new UserNotFoundException(request.ProviderId,nameof(VigigUser.Id));
        
        var deposit = (await _depositRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault() ??
                      throw new DepositNotFoundException(request.Id,nameof(Deposit.Id));
        
        _mapper.Map(request,deposit);
        await _depositRepository.UpdateAsync(deposit);
        await _unitOfWork.CommitAsync();
        
        return new ServiceActionResult(true)
        {
            Data = deposit,
            StatusCode = StatusCodes.Status204NoContent
        };

    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}