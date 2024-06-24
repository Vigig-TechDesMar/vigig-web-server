using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Vigig.Api.Hubs.Models;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Booking;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions;
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
    private readonly IProviderServiceService _providerServiceService;
    private readonly INotificationService _notificationService;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IJwtService _jwtService;

    public BookingHub(BookingConnectionPool pool, 
        IProviderServiceService providerServiceService, 
        IVigigUserRepository vigigUserRepository, 
        INotificationService notificationService, 
        IBookingService bookingService, 
        IJwtService jwtService)
    {
        _pool = pool;
        _bookingService = bookingService;
        _vigigUserRepository = vigigUserRepository;
        _providerServiceService = providerServiceService;
        _jwtService = jwtService;
        _notificationService = notificationService;
    }

    public override Task OnConnectedAsync()
    {
        // var userId = Context.GetHttpContext().Request.Query["userId"].ToString();
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString() ?? throw new InvalidTokenException();
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
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString();
        if (string.IsNullOrEmpty(accessToken)) return base.OnDisconnectedAsync(exception);
        var userId = _jwtService.GetSubjectClaim(accessToken);
        var connectionId = Context.ConnectionId;

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(connectionId))
            return base.OnDisconnectedAsync(exception);
        if (!_pool.connectionPool.TryGetValue(userId, out var connections)) return base.OnDisconnectedAsync(exception);
        lock (connections)
        {
            connections.Remove(connectionId);
            if (connections.Count == 0)
            {
                _pool.connectionPool.TryRemove(userId, out _);
            }
        }
        return base.OnDisconnectedAsync(exception);
    }


    // /booking-hub?bookingid=...
    public async Task<DtoPlacedBooking> PlaceBooking(BookingPlaceRequest request , string redirectUrl)
    {
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString() ?? throw new InvalidTokenException();
       
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
        await Clients.Client(providerConnectionId).SendAsync("ReceiveBooking",dtoPlacedBooking);
        await Clients.Client(providerConnectionId).SendAsync("ReceiveNotification", notifications);
        return dtoPlacedBooking;    
    }
    public async Task<DtoBookingResponse> AcceptBooking(Guid bookingId, string redirectUrl)
    {
        var accessToken= Context.GetHttpContext()?.Request.Query["access_token"].ToString() ?? throw new InvalidTokenException();
        var dtoAcceptedBooking = await _bookingService.RetrievedAcceptBookingAsync(bookingId, accessToken);
        
        
        var message = $"{dtoAcceptedBooking.ProviderName} vừa đồng dịch vụ {dtoAcceptedBooking.ServiceName} của bạn.";
        NotifyClient(dtoAcceptedBooking.ClientId, redirectUrl, message);
        
        _pool.connectionPool.TryGetValue(dtoAcceptedBooking.ClientId.ToString(), out var clientConnectionIds);
        if (clientConnectionIds is null) return dtoAcceptedBooking;
        var clientConnectionId = clientConnectionIds.LastOrDefault();
        if (clientConnectionId is null) return dtoAcceptedBooking;

        var notifications =await _notificationService.RetrieveUserNotification(dtoAcceptedBooking.ClientId);
        await Clients.Client(clientConnectionId).SendAsync("BookingAccepted", dtoAcceptedBooking);
        await Clients.Client(clientConnectionId).SendAsync("ReceiveNotification",notifications);

        return dtoAcceptedBooking;
    }
    public async Task<DtoBookingResponse> DeclineBooking(Guid bookingId, string redirectUrl)
    {
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString() ?? throw new InvalidTokenException();
        var dtoBooking = await _bookingService.RetrievedDeclineBookingAsync(bookingId, accessToken);
        
        var message = $"{dtoBooking.ProviderName} vừa từ chối dịch vụ {dtoBooking.ServiceName} của bạn.";
        NotifyClient(dtoBooking.ClientId, redirectUrl, message);
        
        _pool.connectionPool.TryGetValue(dtoBooking.ClientId.ToString(), out var clientConnectionIds);
        if (clientConnectionIds is null) return dtoBooking;
        var clientConnectionId = clientConnectionIds.LastOrDefault();
        if (clientConnectionId is null) return dtoBooking;
        var notifications = await _notificationService.RetrieveUserNotification(dtoBooking.ClientId);
        await Clients.Client(clientConnectionId).SendAsync("BookingDeclined", dtoBooking);
        await Clients.Client(clientConnectionId).SendAsync("ReceiveNotification", notifications);
        return dtoBooking;
    }
    public async Task<DtoBookingResponse> CompleteBooking(Guid bookingId, BookingCompleteRequest request ,string redirectUrl)
    {
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString() ?? throw new InvalidTokenException();
        var dtoBooking = await _bookingService.RetrievedCompleteBookingAsync(bookingId, request, accessToken);

        var message = $"{dtoBooking.ProviderName} vừa hoàn thành dịch vụ {dtoBooking.ServiceName} của bạn.";
        NotifyClient(dtoBooking.ClientId, redirectUrl, message);

        _pool.connectionPool.TryGetValue(dtoBooking.ClientId.ToString(), out var clientConnectionIds);
        if (clientConnectionIds is null) return dtoBooking;
        var clientConnectionId = clientConnectionIds.LastOrDefault();
        if (clientConnectionId is null) return dtoBooking;

        var notifications = await _notificationService.RetrieveUserNotification(dtoBooking.ClientId);
        
        await Clients.Client(clientConnectionId).SendAsync("BookingCompleted", dtoBooking);
        await Clients.Client(clientConnectionId).SendAsync("ReceiveNotification", notifications);
        return dtoBooking;
    }

    public async Task<DtoBookingResponse> CancelBookingByClient(Guid bookingId, string redirectUrl)
    {
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString() ?? throw new InvalidTokenException();
        var dtoBooking = await _bookingService.RetrievedCancelledByClientBookingAsync(bookingId, accessToken);
        
        var message = $"{dtoBooking.ClientName} vừa hủy dịch vụ {dtoBooking.ServiceName} của bạn.";
        NotifyClient(dtoBooking.ProviderId, redirectUrl, message);
        
        _pool.connectionPool.TryGetValue(dtoBooking.ClientId.ToString(), out var clientConnectionIds);
        if (clientConnectionIds is null) return dtoBooking;
        var clientConnectionId = clientConnectionIds.LastOrDefault();
        if (clientConnectionId is null) return dtoBooking;

        var notifications = await _notificationService.RetrieveUserNotification(dtoBooking.ClientId);
        
        await Clients.Client(clientConnectionId).SendAsync("BookingCompleted", dtoBooking);
        await Clients.Client(clientConnectionId).SendAsync("ReceiveNotification", notifications);
        return dtoBooking;  
    }
    
    public async Task<DtoBookingResponse> CancelBookingByProvider(Guid bookingId, string redirectUrl)
    {
        var accessToken = Context.GetHttpContext()?.Request.Query["access_token"].ToString() ?? throw new InvalidTokenException();
        var dtoBooking = await _bookingService.RetrievedCancelledByProviderBookingAsync(bookingId, accessToken);
        
        var message = $"{dtoBooking.ProviderName} vừa hủy dịch vụ {dtoBooking.ServiceName} của bạn.";
        NotifyClient(dtoBooking.ClientId, redirectUrl, message);
        
        _pool.connectionPool.TryGetValue(dtoBooking.ProviderId.ToString(), out var providerConnectionIds);
        if (providerConnectionIds is null) return dtoBooking;
        var providerConnectionId = providerConnectionIds.LastOrDefault();
        if (providerConnectionId is null) return dtoBooking;
        var notifications = await _notificationService.RetrieveUserNotification(dtoBooking.ProviderId);
        await Clients.Client(providerConnectionId).SendAsync("ReceiveBooking",dtoBooking);
        await Clients.Client(providerConnectionId).SendAsync("ReceiveNotification", notifications);
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

    private void NotifyClient(Guid clientId, string redirectUrl, string message)
    {
        BackgroundJob.Schedule(() => _notificationService.CreateNotification(clientId, message, redirectUrl), TimeSpan.Zero);
    }
}