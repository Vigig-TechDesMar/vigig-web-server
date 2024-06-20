using Hangfire;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Vigig.Api.Hubs.Models;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Booking;
using Vigig.Domain.Entities;
using Vigig.Service.BackgroundJobs.Interfaces;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Request.Booking;
using Vigig.Service.Models.Request.Notification;
using Hub = Microsoft.AspNetCore.SignalR.Hub;

namespace Vigig.Api.Hubs;

public class BookingHub : Hub
{
    private readonly BookingConnectionPool _pool;
    private readonly IBookingService _bookingService;
    private readonly IBookingMessageService _messageService;
    private readonly IProviderServiceService _providerServiceService;
    private readonly INotificationService _notificationService;
    private readonly IBackgroundJobService _backgroundJobService;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IJwtService _jwtService;

    public BookingHub(BookingConnectionPool pool, IBookingService bookingService, IBookingMessageService messageService, IVigigUserRepository vigigUserRepository, IProviderServiceService providerServiceService, IJwtService jwtService, INotificationService notificationService, IBackgroundJobService backgroundJobService)
    {
        _pool = pool;
        _bookingService = bookingService;
        _messageService = messageService;
        _vigigUserRepository = vigigUserRepository;
        _providerServiceService = providerServiceService;
        _jwtService = jwtService;
        _notificationService = notificationService;
        _backgroundJobService = backgroundJobService;
    }

    public override Task OnConnectedAsync()
    {
        // var userId = Context.GetHttpContext().Request.Query["userId"].ToString();
        var accessToken = Context.GetHttpContext().Request.Query["access_token"].ToString();
        var userId = _jwtService.GetSubjectClaim(accessToken).ToString();
        var connectionId = Context.ConnectionId;

        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(connectionId))
        {
            if (!_pool.connectionPool.ContainsKey(userId))
            {
                _pool.connectionPool[userId] = new List<string>();
            }
            _pool.connectionPool[userId].Add(connectionId);
        }

        return base.OnConnectedAsync();
    }
    
    
    
    // /booking-hub?bookingid=...
    public async Task<DtoPlacedBooking> PlaceBooking(BookingPlaceRequest request , string redirectUrl)
    {
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString();
       
        var providerService = await _providerServiceService.RetrieveProviderServiceByIdAsync(request.ProviderServiceId);

        var provider = await _vigigUserRepository.GetAsync(x => x.Id == providerService.ProviderId)
                       ?? throw new UserNotFoundException(providerService.ProviderId,nameof(ProviderService.Id));
        var dtoPlacedBooking = await _bookingService.RetrievedPlaceBookingAsync(accessToken, request);
        NotifyProvider(dtoPlacedBooking.Id, request.BookerName, dtoPlacedBooking.ProviderServiceName, redirectUrl);
        
        _pool.connectionPool.TryGetValue(provider.Id.ToString(), out var providerConnectionIds);
        if (providerConnectionIds is null) return dtoPlacedBooking;
        var providerConnectionId = providerConnectionIds.LastOrDefault();
        if (providerConnectionId is null) return dtoPlacedBooking;
        var notifications = await _notificationService.RetrieveUserNotification(provider.Id);
        Clients.Client(providerConnectionId)?.SendAsync("ReceiveBooking",dtoPlacedBooking);
        return dtoPlacedBooking;    
    }

    public async Task<DtoAcceptedBooking> AcceptBooking(Guid bookingId, string redirectUrl)
    {
        var accessToken= Context.GetHttpContext()?.Request.Query["access_token"].ToString();
        var dtoAcceptedBooking = await _bookingService.RetrievedAcceptBookingAsync(bookingId, accessToken);
        
        
        var message = $"{dtoAcceptedBooking.ProviderName} vừa đồng dịch vụ {dtoAcceptedBooking.ServiceName} của bạn.";
        NotifyClient(dtoAcceptedBooking.ClientId, dtoAcceptedBooking.ProviderName, dtoAcceptedBooking.ServiceName, redirectUrl, message);
        
        _pool.connectionPool.TryGetValue(dtoAcceptedBooking.ClientId.ToString(), out var clientConnectionIds);
        if (clientConnectionIds is null) return dtoAcceptedBooking;
        var clientConnectionId = clientConnectionIds.LastOrDefault();
        if (clientConnectionId is null) return dtoAcceptedBooking;

        var notifications =await _notificationService.RetrieveUserNotification(dtoAcceptedBooking.ClientId);
        Clients.Client(clientConnectionId)?.SendAsync("BookingAccepted", dtoAcceptedBooking);
        Clients.Client(clientConnectionId)?.SendAsync("ReceiveNotification",notifications);

        return dtoAcceptedBooking;
    }

    public async Task<DtoAcceptedBooking> DeclinedBooking(Guid bookingId, string redirectUrl)
    {
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString();
        var dtoBooking = await _bookingService.RetrievedDeclineBookingAsync(bookingId, redirectUrl);
        
        var message = $"{dtoBooking.ProviderName} vừa từ chối dịch vụ {dtoBooking.ServiceName} của bạn.";
        NotifyClient(dtoBooking.ClientId, dtoBooking.ProviderName, dtoBooking.ServiceName, redirectUrl, message);
        
        _pool.connectionPool.TryGetValue(dtoBooking.ClientId.ToString(), out var clientConnectionIds);
        if (clientConnectionIds is null) return dtoBooking;
        var clientConnectionId = clientConnectionIds.LastOrDefault();
        if (clientConnectionId is null) return dtoBooking;
        Clients.Client(clientConnectionId)?.SendAsync("BookingAccepted", dtoBooking);
        return dtoBooking;
    }


    private void NotifyProvider(Guid bookingId, string bookerName, string serviceName, string redirectUrl)
    {
        var notificationRequest = new CreateBookingNotificationRequest()
        {
            Content = $"{bookerName} vừa đặt dịch vụ {serviceName} của bạn",
            BookingId = bookingId,
            RedirectUrl = redirectUrl
        };
        BackgroundJob.Schedule(() => _notificationService.CreateBookingNotification(notificationRequest),TimeSpan.Zero);
    }

    private void NotifyClient(Guid clientId, string providerName, string serviceName, string redirectUrl, string message)
    {
        BackgroundJob.Schedule(() => _notificationService.CreateNotification(clientId, message, redirectUrl), TimeSpan.Zero);
    }
}