using System.Transactions;
using AutoMapper;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Wallet;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Wallet;
using IWalletRepository = Vigig.DAL.Interfaces.IWalletRepository;
using Transaction = Vigig.Domain.Entities.Transaction;

namespace Vigig.Service.Implementations;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBookingFeeRepository _bookingFeeRepository;
    private readonly ISubscriptionFeeRepository _subscriptionFeeRepository;
    private readonly IDepositRepository _depositRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransactionService(ITransactionRepository transactionRepository, IBookingFeeRepository bookingFeeRepository, ISubscriptionFeeRepository subscriptionFeeRepository, IDepositRepository depositRepository, IWalletRepository walletRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _bookingFeeRepository = bookingFeeRepository;
        _subscriptionFeeRepository = subscriptionFeeRepository;
        _depositRepository = depositRepository;
        _walletRepository = walletRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var transaction = _mapper.ProjectTo<DtoTransaction>(await _transactionRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = transaction
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var transaction = (await _transactionRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault();
        if (transaction is null)
            throw new TransactionNotFoundException(id,nameof(Transaction.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoTransaction>(transaction)
        };

    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var transactions = _mapper.ProjectTo<DtoTransaction>(
            await _transactionRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoTransaction>(transactions, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchTransaction(SearchUsingGet request)
    {
        var transactions = (await _transactionRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoTransaction>>(
                SearchHelper.BuildSearchResult<Transaction>(transactions, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }
    
    //BUSINESS
    public async Task<ServiceActionResult> ProcessTransactionAsync(Domain.Entities.Transaction transaction)
    {
        var userWallet = (await _walletRepository.FindAsync(w => w.Id == transaction.WalletId)).FirstOrDefault();
        if (userWallet == null)
        {
            transaction.Status = TransactionStatusConstant.Pending;
            throw new InvalidOperationException("User wallet not found.");
        }

        if (userWallet.Balance < transaction.Amount)
        {
            transaction.Status = TransactionStatusConstant.Pending;
            throw new InvalidOperationException("Insufficient balance.");
        }

        userWallet.Balance -= transaction.Amount;
        transaction.Status = TransactionStatusConstant.Completed;
        
        

        return null;
    }

    // public async Task<ServiceActionResult> AddAsync(CreateTransactionRequest request)
    // {
    //     return new ServiceActionResult(true)
    //     {
    //
    //     };
    // }
    //
    // public async Task<ServiceActionResult> UpdateAsync(UpdateTransactionRequest request)
    // {
    //     return new ServiceActionResult(true)
    //     {
    //
    //     };
    // }
    //
    // public async Task<ServiceActionResult> DeleteAsync(Guid id)
    // {
    //     throw new NotImplementedException();
    // }
}