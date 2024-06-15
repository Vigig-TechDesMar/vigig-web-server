using Vigig.DAL.Interfaces;
using Vigig.Service.BackgroundJobs.Interfaces;
using Vigig.Service.Interfaces;

namespace Vigig.Service.BackgroundJobs;

public class ExpirationService : IExpirationService
{
    private readonly IEventRepository _eventRepository;
    private readonly IPopUpRepository _popUpRepository;
    private readonly IBannerRepository _bannerRepository;
    private readonly ILeaderBoardRepository _leaderBoardRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly ISubscriptionFeeRepository _subscriptionFeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExpirationService(IEventRepository eventRepository, IUnitOfWork unitOfWork, IPopUpRepository popUpRepository, IBannerRepository bannerRepository, ILeaderBoardRepository leaderBoardRepository, IVoucherRepository voucherRepository, ISubscriptionFeeRepository subscriptionFeeRepository)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _popUpRepository = popUpRepository;
        _bannerRepository = bannerRepository;
        _leaderBoardRepository = leaderBoardRepository;
        _voucherRepository = voucherRepository;
        _subscriptionFeeRepository = subscriptionFeeRepository;
    }
    public async Task ValidateEventExpiration()
    {
        await ValidateEvent();
        await ValidateBanner();
        await ValidateVoucher();
        await ValidatePopUp();
        await ValidateLeaderBoard();
    }

    public Task ValidateSubscriptionFee()
    {
        throw new NotImplementedException();
    }

    #region ValidateEvent
    private async Task ValidateEvent()
    {
        var events = await _eventRepository.FindAsync(e => e.EndDate < DateTime.Now && e.IsActive);
        foreach (var @event in events)
        {
            @event.IsActive = false;
        }
        await _eventRepository.UpdateManyAsync(events);
        await _unitOfWork.CommitAsync();
    }
    private async Task ValidateBanner()
    {
        var banners = await _bannerRepository.FindAsync(b => b.EndDate < DateTime.Now && b.IsActive);
        foreach (var @banner in banners)
        {
            @banner.IsActive = false;
        }
        await _bannerRepository.UpdateManyAsync(banners);
        await _unitOfWork.CommitAsync();
    }

    private async Task ValidatePopUp()
    {
        var popups = await _popUpRepository.FindAsync(p => p.EndDate < DateTime.Now && p.IsActive);
        foreach (var @popup in popups)
        {
            @popup.IsActive = false;    
        }
        await _popUpRepository.UpdateManyAsync(popups);
        await _unitOfWork.CommitAsync();
    }

    private async Task ValidateVoucher()
    {
        var vouchers = await _voucherRepository.FindAsync(v => v.EndDate < DateTime.Now && v.IsActive);
        foreach (var voucher in vouchers)
        {
            @voucher.IsActive = false;
        }
        await _voucherRepository.UpdateManyAsync(vouchers);
        await _unitOfWork.CommitAsync();
    }

    private async Task ValidateLeaderBoard()
    {
        var leaderBoards = await _leaderBoardRepository.FindAsync(l => l.EndDate < DateTime.Now && l.IsActive);
        foreach (var @leaderBoard in leaderBoards)
        {
            @leaderBoard.IsActive = false;
        }
        await _leaderBoardRepository.UpdateManyAsync(leaderBoards);      
        await _unitOfWork.CommitAsync();
    }

    #endregion
    
}