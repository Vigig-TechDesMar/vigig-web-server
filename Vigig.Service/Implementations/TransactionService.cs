using System.Transactions;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using Org.BouncyCastle.Security;
using Vigig.Common.Exceptions;
using Vigig.Common.Helpers;
using Vigig.Common.Settings;
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
using Vigig.Service.Models.Response;
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
    private readonly PayOSSetting _payOsSetting;
    private readonly IConfiguration _configuration;

    public TransactionService(ITransactionRepository transactionRepository, IBookingFeeRepository bookingFeeRepository, ISubscriptionFeeRepository subscriptionFeeRepository, IDepositRepository depositRepository, IWalletRepository walletRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _transactionRepository = transactionRepository;
        _bookingFeeRepository = bookingFeeRepository;
        _subscriptionFeeRepository = subscriptionFeeRepository;
        _depositRepository = depositRepository;
        _walletRepository = walletRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
        _payOsSetting = _configuration.GetSection(nameof(PayOSSetting)).Get<PayOSSetting>() ?? throw new MissingPayOSSetting();
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var transaction = _mapper.ProjectTo<DtoTransaction>(await _transactionRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = transaction
        };
    }

    public async Task<ServiceActionResult> GetById(int id)
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
            CreatedDate = DateTime.Now,
            Description = GetTransactionDescription(feeType),
            Status = TransactionStatusConstant.Pending,
            WalletId = wallet.Id
        };

        AssignFeeIdToTransaction(fee, transaction);

        await _transactionRepository.AddAsync(transaction);
        await _unitOfWork.CommitAsync();
        
        await PerformTransactionAsync(transaction, fee);
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
        }
        else
        {
            try
            {
                if (wallet.Balance < transaction.Amount)
                    throw new InvalidOperationException("Insufficient balance.");
                
                wallet.Balance -= transaction.Amount;
                transaction.Status = TransactionStatusConstant.Completed;
                fee.Status = CashStatus.Success;
            }
            catch (InvalidOperationException e)
            {
               
                transaction.Status = TransactionStatusConstant.Failed;
                fee.Status = CashStatus.Fail;
                await _unitOfWork.CommitAsync();
                throw e;
            }
        }

        await _walletRepository.UpdateAsync(wallet);
        await _unitOfWork.CommitAsync();
    }

    private async Task PayOSProcess(Transaction transaction)
    {
        //VALIDATION
        if (transaction.Amount < 0)
            throw new InvalidParameterException("Amount cannot be negative");
        PayOS payOS = new PayOS(_payOsSetting.ClientId, _payOsSetting.ApiKey, _payOsSetting.ChecksumKey); 
        
        //Confirm Webhook
        await payOS.confirmWebhook(_payOsSetting.Webhook);
            
        ItemData item = new ItemData("Vigig Wallet Deposition", 1, Convert.ToInt32(transaction.Amount));
        List<ItemData> items = new List<ItemData>();
        items.Add(item);
        PaymentData paymentData = new PaymentData(Convert.ToInt32(transaction.Id), Convert.ToInt32(transaction.Amount), "Nap vi Vigig",
            items, _payOsSetting.CancelUrl, _payOsSetting.ReturnUrl);
        
        CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
        Console.WriteLine("\n *********************************************************** \n");
        Console.WriteLine(createPayment);
    }

    public async Task<Response> ProcessPayOSReturnResult(WebhookType request)
    {
        PayOS payOS = new PayOS(_payOsSetting.ClientId, _payOsSetting.ApiKey, _payOsSetting.ChecksumKey); 

        WebhookData webhookData = payOS.verifyPaymentWebhookData(request);
        Response returnObject = new Response();
        if (webhookData.description == "Ma giao dich thu nghiem")
            return new Response
            {
                Error = 0,
                Data = null,
                Message = "OK"
            };
                
        if (webhookData.code == "00") //Success
        {
            Transaction transaction = (await _transactionRepository.FindAsync(sc=> sc.Id == webhookData.orderCode)).FirstOrDefault()??
                                      throw new TransactionNotFoundException(webhookData.orderCode.ToString(),nameof(Transaction));

            await UpdateUponSuccessfulDeposit(transaction);
        }

        return new Response();
    }

    private async Task UpdateUponSuccessfulDeposit(Transaction transaction)
    {
        if (transaction.DepositId is null) return;
        
        //Deposit
        var deposit = (await _depositRepository.FindAsync(sc => sc.Id == transaction.DepositId)).FirstOrDefault() ??
                      throw new DepositNotFoundException(transaction.DepositId, nameof(Deposit));

        var wallet = (await _walletRepository.FindAsync(sc => sc.Id == transaction.WalletId)).FirstOrDefault() ??
                     throw new WalletNotFoundException(transaction.WalletId, nameof(Wallet));
        
        //Update the Wallet
        wallet.Balance += Math.Round(transaction.Amount/1000);
        transaction.Status = TransactionStatusConstant.Completed;
        deposit.Status = CashStatus.Success;

        await _unitOfWork.CommitAsync();
    }
}