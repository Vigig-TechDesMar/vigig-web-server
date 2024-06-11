using System.Transactions;
using AutoMapper;
using Net.payOS;
using Net.payOS.Types;
using Org.BouncyCastle.Security;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Wallet;
using Vigig.Domain.Entities;
using Vigig.Domain.Entities.BaseEntities;
using Vigig.Domain.Enums;
using Vigig.Domain.Interfaces;
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
   public async Task ProcessTransactionAsync(CashEntity fee, Wallet wallet)
    {
        var feeType = fee.GetType().Name;
        var transaction = new Transaction
        {
            Amount = fee.Amount,
            CreatedDate = DateTime.UtcNow,
            Description = GetTransactionDescription(feeType),
            Status = TransactionStatusConstant.Pending,
            Wallet = wallet,
            WalletId = wallet.Id
        };

        AssignFeeIdToTransaction(fee, transaction);

        await _transactionRepository.AddAsync(transaction);
        await _unitOfWork.CommitAsync();

        try
        {
            await PerformTransactionAsync(transaction, fee);
        }
        catch (Exception ex)
        {
            fee.Status = CashStatus.Fail;
            // Log the exception (consider using a logging framework)
            // Example: _logger.LogError(ex, "Error performing transaction");
            throw;
        }
    }

    private void AssignFeeIdToTransaction(CashEntity fee, Transaction transaction)
    {
        switch (fee.GetType().Name)
        {
            case nameof(SubscriptionFee):
                transaction.SubscriptionFeeId = fee.Id;
                break;

            case nameof(BookingFee):
                transaction.BookingFeeId = fee.Id;
                break;

            case nameof(Deposit):
                transaction.DepositId = fee.Id;
                break;
        }
    }

    private string GetTransactionDescription(string feeType)
    {
        return feeType switch
        {
            CashTypeConstant.SubscriptionFee => "Subscription Fee Transaction",
            CashTypeConstant.BookingFee => "Booking Fee Transaction",
            CashTypeConstant.Deposit => "Deposit Transaction",
            _ => "Transaction"
        };
    }

    private async Task PerformTransactionAsync(Transaction transaction, CashEntity fee)
    {
        var wallet = transaction.Wallet;
        
        if (wallet == null)
        {
            transaction.Status = TransactionStatusConstant.Failed;
            throw new InvalidOperationException("User wallet not found.");
        }

        if (fee.GetType().Name == CashTypeConstant.Deposit)
        {
            //PayOS Business Logic
            await PayOSProcess(transaction);
            
            //Update the Wallet
            wallet.Balance += transaction.Amount;
            transaction.Status = TransactionStatusConstant.Completed;
            fee.Status = CashStatus.Success;
        }
        else
        {
            if (wallet.Balance < transaction.Amount)
            {
                transaction.Status = TransactionStatusConstant.Failed;
                fee.Status = CashStatus.Fail;
                throw new InvalidOperationException("Insufficient balance.");
            }

            wallet.Balance -= transaction.Amount;
            transaction.Status = TransactionStatusConstant.Completed;
            fee.Status = CashStatus.Success;
        }

        await _walletRepository.UpdateAsync(wallet);
        await _unitOfWork.CommitAsync();
    }

    private async Task PayOSProcess(Transaction transaction)
    {
        //VALIDATION
        if (transaction.Amount < 0)
            throw new InvalidParameterException("Amount cannot be negative");
        
        const string clientId = "23209feb-3f68-4c93-9d2d-8b43957b210f";
        const string apiKey = "ad1f123e-10cb-4ebd-b478-6a583a4ac095";
        const string checksumKey = "5fde396e4e45303c1fee7781b6ba36bf22e5a96979b9a5b1bf76dc1bc75c1980";
        const string cancelUrl = "https://localhost:3002";
        const string returnUrl = "https://localhost:3002";
        
        PayOS payOS = new PayOS(clientId, apiKey, checksumKey);
        
        ItemData item = new ItemData("Vigig Wallet Deposition", 1, Convert.ToInt32(transaction.Amount));
        List<ItemData> items = new List<ItemData>();
        items.Add(item);
    
        PaymentData paymentData = new PaymentData(Convert.ToInt32(transaction.Id), Convert.ToInt32(transaction.Amount), "Nap vi Vigig",
            items, cancelUrl, returnUrl);

        CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
        Console.WriteLine(createPayment);
    }
}