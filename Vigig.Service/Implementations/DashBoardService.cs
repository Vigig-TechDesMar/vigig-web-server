using Microsoft.EntityFrameworkCore;
using Vigig.Common.Constants;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;
using Vigig.Domain.Enums;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.DashBoard;
using Vigig.Service.Models.Response.DashBoard;

namespace Vigig.Service.Implementations;

public class DashBoardService : IDashBoardService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ISubscriptionFeeRepository _subscriptionFeeRepository;
    private readonly IBookingFeeRepository _bookingFeeRepository;
    private readonly IDepositRepository _depositRepository;
    private readonly IComplaintRepository _complaintRepository;
    private readonly ILeaderBoardRepository _leaderBoardRepository;
    private readonly IProviderKPIRepository _providerKpiRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IBannerRepository _bannerRepository;
    private readonly IPopUpRepository _popUpRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IJwtService _jwtService;

    public DashBoardService(IBookingRepository bookingRepository, ISubscriptionFeeRepository subscriptionFeeRepository, IDepositRepository depositRepository, IComplaintRepository complaintRepository, ILeaderBoardRepository leaderBoardRepository, IVigigUserRepository vigigUserRepository, IEventRepository eventRepository, IBannerRepository bannerRepository, IPopUpRepository popUpRepository, IVoucherRepository voucherRepository, ITransactionRepository transactionRepository, IJwtService jwtService, IBookingFeeRepository bookingFeeRepository, IProviderKPIRepository providerKpiRepository, IWalletRepository walletRepository)
    {
        _bookingRepository = bookingRepository;
        _subscriptionFeeRepository = subscriptionFeeRepository;
        _depositRepository = depositRepository;
        _complaintRepository = complaintRepository;
        _leaderBoardRepository = leaderBoardRepository;
        _vigigUserRepository = vigigUserRepository;
        _eventRepository = eventRepository;
        _bannerRepository = bannerRepository;
        _popUpRepository = popUpRepository;
        _voucherRepository = voucherRepository;
        _transactionRepository = transactionRepository;
        _jwtService = jwtService;
        _bookingFeeRepository = bookingFeeRepository;
        _providerKpiRepository = providerKpiRepository;
        _walletRepository = walletRepository;
    }

    public async Task<ServiceActionResult> ProviderGetBookings(DashBoardRequest request, string token)
    {
        var authModel = VerifyToken(token);
        if (authModel.Role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!");

        var bookingList = (await _bookingRepository.FindAsync(
                x => x.IsActive && 
                     x.ProviderService.ProviderId == authModel.UserId &&
                     x.CreatedDate <= request.EndDate &&
                     x.CreatedDate >= request.StartDate))
            .Include(x => x.ProviderService) // Include ProviderService
            .AsQueryable();
        
        var completedBookingNo = 0;
        var cancelledBookingNo = 0;
        var timeoutBookingNo = 0;
        var declinedBookingNo = 0;
        foreach (var booking in bookingList)
        {
            switch (booking.Status)
            {
                case BookingStatus.Completed:
                    completedBookingNo++;
                    break;
                
                case BookingStatus.CancelledByClient:
                    cancelledBookingNo++;
                    break;
                
                case BookingStatus.CancelledByProvider:
                    cancelledBookingNo++;
                    break;
                
                case BookingStatus.Timeout:
                    timeoutBookingNo++;
                    break;
                
                case BookingStatus.Declined:
                    declinedBookingNo++;
                    break;
            }
        }

        var response = new ProviderBookingDashBoardResponse
        {
            CompletedBooking = completedBookingNo,
            CancelledBooking = cancelledBookingNo,
            TimeoutBooking = timeoutBookingNo,
            DeclinedBooking = declinedBookingNo
        };
        
        return new ServiceActionResult(true)
        {
            Data = response
        };
    }
    
    public async Task<ServiceActionResult> ProviderGetTotalCashFlow(DashBoardRequest request, string token)
    {
        var authModel = VerifyToken(token);
        if (authModel.Role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!"); 
        
        var bookingList = (await _bookingRepository.FindAsync(x => 
                x.IsActive && 
                x.Status == BookingStatus.Completed && 
                x.CreatedDate <= request.EndDate && 
                x.CreatedDate >= request.StartDate))
            .Include(x => x.ProviderService) // Include ProviderService
            .Where(x => x.ProviderService.ProviderId == authModel.UserId) // Filter by ProviderId
            .AsQueryable();

        double totalRevenue = bookingList.Sum(x => x.FinalPrice);

        var bookingFeeList = (await _bookingFeeRepository.FindAsync(
                sc => sc.Booking.ProviderService.ProviderId == authModel.UserId &&
                      sc.Status == CashStatus.Success &&
                      sc.CreatedDate <= request.EndDate &&
                      sc.CreatedDate >= request.StartDate))
            .Include(sc => sc.Booking) // Include Booking
            .ThenInclude(b => b.ProviderService) // Include ProviderService in Booking
            .AsQueryable();
            
        double bookingFee = bookingFeeList.Sum(sc => sc.Amount);

        var subscriptionFeeList =
            (await _subscriptionFeeRepository.FindAsync(sc =>
                sc.ProviderId == authModel.UserId && sc.Status == CashStatus.Success && sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate)).AsQueryable();
        double subscriptionFee = subscriptionFeeList.Sum(x => x.Amount);
        
        var deposits =
            (await _depositRepository.FindAsync(sc =>
                sc.ProviderId == authModel.UserId && sc.Status == CashStatus.Success && sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate)).AsQueryable();
        double deposit = deposits.Sum(x=> x.Amount);
        
        var response = new ProviderCashFlowDashBoardResponse
        {
            Revevue = totalRevenue,
            BookingFee = bookingFee,
            Deposit = deposit,
            SubscriptionFee = subscriptionFee
        };
        return new ServiceActionResult(true)
        {
            Data = response
        };
    }

    public async Task<ServiceActionResult> ProviderGetComplaints(DashBoardRequest request, string token)
    {
        var authModel = VerifyToken(token);
        if (authModel.Role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!");
        var complaintNo = (await _complaintRepository.FindAsync(sc =>
                sc.BookingId != null && 
                sc.Booking.ProviderService.ProviderId == authModel.UserId))
            .Include(sc => sc.Booking) // Include Booking
            .ThenInclude(b => b.ProviderService) // Include ProviderService within Booking
            .Count();

        return new ServiceActionResult(true)
        {
            Data = complaintNo
        };
    }

    public async Task<ServiceActionResult> ProviderGetLeaderBoard(DashBoardRequest request, string token)
    {
        var authModel = VerifyToken(token);
        if (authModel.Role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!");

        var kpis =
            (await _providerKpiRepository.FindAsync(sc => 
                sc.ProviderId == authModel.UserId && sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate))
                .Include(x => x.LeaderBoard);
        
        return new ServiceActionResult(true)
        {
            Data = kpis
        };
    }

    public async Task<ServiceActionResult> ProviderGetTransaction(DashBoardRequest request, string token)
    {
        var authModel = VerifyToken(token);
        if (authModel.Role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!");

        var wallet = (await _walletRepository.FindAsync(sc => sc.ProviderId == authModel.UserId)).FirstOrDefault();

        if (wallet == null)
            throw new WalletNotFoundException(authModel.UserId, nameof(Transaction.WalletId));
        
        var transactions = (await _transactionRepository.FindAsync(sc =>
            sc.WalletId == wallet.Id && sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate));

        var success = 0;
        var pending = 0;
        var fail = 0;
        
        foreach (var transaction in transactions)
        {
            switch (transaction.Status)
            {
                case CashStatusConstant.SuccessInt:
                    success++;
                    break;
                
                case CashStatusConstant.PendingInt:
                    pending++;
                    break;
                
                case CashStatusConstant.FailInt:
                    fail++;
                    break;
            }
        }

        return new ServiceActionResult(true)
        {
            Data = new TransactionDashBoardResponse
            {
                Success = success,
                Pending = pending,
                Failure = fail
            }
        };
    }

    public async Task<ServiceActionResult> AdminGetBookings(DashBoardRequest request, string token)
    {
        var authModel = VerifyToken(token);
        var bookingList = (await _bookingRepository.FindAsync(x => x.IsActive && x.CreatedDate <= request.EndDate && x.CreatedDate >= request.StartDate)).AsQueryable();
        
        var completedBookingNo = 0;
        var cancelledByProviderBookingNo = 0;
        var cancelledByClientBookingNo = 0;
        var timeoutBookingNo = 0;
        var declinedBookingNo = 0;
        foreach (var booking in bookingList)
        {
            switch (booking.Status)
            {
                case BookingStatus.Completed:
                    completedBookingNo++;
                    break;
                
                case BookingStatus.CancelledByClient:
                    cancelledByClientBookingNo++;
                    break;
                
                case BookingStatus.CancelledByProvider:
                    cancelledByProviderBookingNo++;
                    break;
                
                case BookingStatus.Timeout:
                    timeoutBookingNo++;
                    break;
                
                case BookingStatus.Declined:
                    declinedBookingNo++;
                    break;
            }
        }

        var response = new AdminBookingDashBoardResponse()
        {
            CompletedBooking = completedBookingNo,
            CancelledByClientBooking = cancelledByClientBookingNo,
            CancelledByProviderBooking = cancelledByProviderBookingNo,
            TimeoutBooking = timeoutBookingNo,
            DeclinedBooking = declinedBookingNo
        };
        
        return new ServiceActionResult(true)
        {
            Data = response
        };
    }
    
    public async Task<ServiceActionResult> AdminGetComplaints(DashBoardRequest request, string token)
    {
        var authModel = VerifyToken(token);
        var bookingComplaintNo =
            (await _complaintRepository.FindAsync(sc => sc.BookingId != null)).Count();

        var systemComplaintNo = (await _complaintRepository.FindAsync(sc => sc.BookingId == null)).Count();
        return new ServiceActionResult(true)
        {
            Data = new AdminComplaintDashBoard
            {
                SystemComplaint = systemComplaintNo,
                BookingComplaint = bookingComplaintNo
            }
        };    
    }
    
    public async Task<ServiceActionResult> AdminGetNewUsers(DashBoardRequest request, string token)
    {
        var newUsers = (await _vigigUserRepository.FindAsync(sc =>
            sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate && sc.IsActive));
        var newClientNo = newUsers.Count(y=> y.Roles.Select(x=> x.NormalizedName).Contains(UserRoleConstant.Client));
        var newProviderNo = newUsers.Count(y=> y.Roles.Select(x=> x.NormalizedName).Contains(UserRoleConstant.Provider));
        return new ServiceActionResult(true)
        {
            Data = new AdminNewUserDashBoardResponse
            {
                NewClientsNo = newClientNo,
                NewProvidersNo = newProviderNo
            }
        };
    }

    public async Task<ServiceActionResult> AdminGetTotalRevenue(DashBoardRequest request, string token)
    {
        double bookingFeeSuccess = 0;
        double bookingFeeFail = 0;
        double bookingFeePending = 0;
        double bookingFeeSuccessNo = 0;
        double bookingFeeFailNo = 0;
        double bookingFeePendingNo = 0;
        
        double subscriptionFeeSuccess = 0;
        double subscriptionFeeFail = 0;
        double subscriptionFeePending = 0;
        double subscriptionFeeSuccessNo = 0;
        double subscriptionFeeFailNo = 0;
        double subscriptionFeePendingNo = 0;
        
        double depositSuccess = 0;
        double depositFail = 0;
        double depositPending = 0;
        double depositSuccessNo = 0;
        double depositFailNo = 0;
        double depositPendingNo = 0;
        
        var bookingFeeList =
            (await _bookingFeeRepository.FindAsync(sc => sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate)).AsQueryable();
        foreach (var bookingFee in bookingFeeList)
        {
            switch (bookingFee.Status)
            {
                case CashStatus.Success:
                    bookingFeeSuccess += bookingFee.Amount;
                    bookingFeeSuccessNo++;
                    break;
                
                case CashStatus.Fail:
                    bookingFeeFail += bookingFee.Amount;
                    bookingFeeFailNo++;
                    break;
                
                case CashStatus.Pending:
                    bookingFeePending += bookingFee.Amount;
                    bookingFeePendingNo++;
                    break;
            }
        }
        
        var subscriptionFeeList =
            (await _subscriptionFeeRepository.FindAsync(sc => sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate)).AsQueryable();
        foreach (var subscriptionFee in subscriptionFeeList)
        {
            switch (subscriptionFee.Status)
            {
                case CashStatus.Success:
                    subscriptionFeeSuccess += subscriptionFee.Amount;
                    subscriptionFeeSuccessNo++;
                    break;
                
                case CashStatus.Fail:
                    subscriptionFeeFail += subscriptionFee.Amount;
                    subscriptionFeeFailNo++;
                    break;
                
                case CashStatus.Pending:
                    subscriptionFeePending += subscriptionFee.Amount;
                    subscriptionFeePendingNo++;
                    break;
            }
        }
        
        var deposits =
            (await _depositRepository.FindAsync(sc => sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate)).AsQueryable();
        foreach (var deposit in deposits)
        {
            switch (deposit.Status)
            {
                case CashStatus.Success:
                    depositSuccess += deposit.Amount;
                    depositSuccessNo++;
                    break;
                
                case CashStatus.Fail:
                    depositFail += deposit.Amount;
                    depositFailNo++;
                    break;
                
                case CashStatus.Pending:
                    depositPending += deposit.Amount;
                    depositPendingNo++;
                    break;
            }
        }
        
        var cashFlow = new AdminCashFlowDashBoard
        {
            Revevue = subscriptionFeeSuccess + bookingFeeSuccess,
            BookingFeeSuccess = bookingFeeSuccess,
            BookingFeeFail = bookingFeeFail,
            BookingFeePending = bookingFeePending,
            BookingFeeSuccessNo = bookingFeeSuccessNo,
            BookingFeeFailNo = bookingFeeFailNo,
            BookingFeePendingNo = bookingFeePendingNo,
            SubscriptionFeeSuccess = subscriptionFeeSuccess,
            SubscriptionFeeFail = subscriptionFeeFail,
            SubscriptionFeePending = subscriptionFeePending,
            SubscriptionFeeSuccessNo = subscriptionFeeSuccessNo,
            SubscriptionFeeFailNo = subscriptionFeeFailNo,
            SubscriptionFeePendingNo = subscriptionFeePendingNo,
            DepositSucces = depositSuccess,
            DepositFail = depositFail,
            DepositPending = depositPending,
            DepositSuccesNo = depositSuccessNo,
            DepositFailNo = depositFailNo,
            DepositPendingNo = depositPendingNo
        };
        return new ServiceActionResult(true)
        {
            Data = cashFlow
        };
    }

    public async Task<ServiceActionResult> AdminGetEventsAndChildren(DashBoardRequest request, string token)
    {
        var events = (await _eventRepository.FindAsync(sc => 
            sc.StartDate <= request.EndDate || sc.EndDate >= request.StartDate));
        
        var leaderBoards = (await _leaderBoardRepository.FindAsync(sc => 
            sc.StartDate <= request.EndDate || sc.EndDate >= request.StartDate));

        var banners = (await _bannerRepository.FindAsync(sc =>
            sc.StartDate <= request.EndDate || sc.EndDate >= request.StartDate));

        var popUps = (await _popUpRepository.FindAsync(sc =>
            sc.StartDate <= request.EndDate || sc.EndDate >= request.StartDate));
        
        return new ServiceActionResult(true)
        {
            Data = new AdminEventMetricsDashBoard
            {
                Events = events.Count(),
                LeaderBoard = leaderBoards.Count(),
                Banner = banners.Count(),
                PopUp = popUps.Count()
            }
        };
    }

    public async Task<ServiceActionResult> AdminGetVoucher(DashBoardRequest request, string token)
    {
        var vouchers = (await _voucherRepository.FindAsync(sc =>
            sc.StartDate <= request.EndDate || sc.EndDate >= request.StartDate));
        return new ServiceActionResult(true)
        {
            Data = vouchers.Count()
        };
    }

    public async Task<ServiceActionResult> AdminGetTransaction(DashBoardRequest request, string token)
    {
        var transactions = (await _transactionRepository.FindAsync(sc =>
            sc.CreatedDate <= request.EndDate && sc.CreatedDate >= request.StartDate));

        var success = 0;
        var pending = 0;
        var fail = 0;
        
        foreach (var transaction in transactions)
        {
            switch (transaction.Status)
            {
                case CashStatusConstant.SuccessInt:
                    success++;
                    break;
                
                case CashStatusConstant.PendingInt:
                    pending++;
                    break;
                
                case CashStatusConstant.FailInt:
                    fail++;
                    break;
            }
        }

        return new ServiceActionResult(true)
        {
            Data = new TransactionDashBoardResponse
            {
                Success = success,
                Pending = pending,
                Failure = fail
            }
        };
    }

    private AuthModel VerifyToken(string token)
    {
        var authModel = _jwtService.GetAuthModel(token);
        return authModel;
    }
}