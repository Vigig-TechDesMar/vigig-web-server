﻿using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Vigig.Api.Hubs.Models;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Booking;
using Vigig.Domain.Entities;
using Vigig.Service.BackgroundJobs.Interfaces;
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
        _pool.connectionPool.TryGetValue(provider.Id.ToString(), out var providerConnectionIds);
        
        NotifyProvider(dtoPlacedBooking.Id, request.BookerName, dtoPlacedBooking.ProviderServiceName, redirectUrl);
        
        if (providerConnectionIds is null) return dtoPlacedBooking;
        var providerConnectionId = providerConnectionIds.LastOrDefault();
        var notifications = await _notificationService.RetrieveProviderNotification(provider.Id);
        if (providerConnectionId is null) return dtoPlacedBooking;
        Clients.Client(providerConnectionId)?.SendAsync("ReceiveBooking",dtoPlacedBooking);
        Clients.Client(providerConnectionId)?.SendAsync("ReceiveNotification",notifications);

        return dtoPlacedBooking;    
    }

    public async Task<DtoAcceptedBooking> AcceptBooking(Guid bookingId, string redirectUrl)
    {
        var accessToken= Context.GetHttpContext()?.Request.Query["access_token"].ToString();
        var dtoAcceptedBooking = await _bookingService.RetrievedAcceptBookingAsync(bookingId, accessToken);

        _pool.connectionPool.TryGetValue(dtoAcceptedBooking.ClientId.ToString(), out var clientConnectionIds);
        NotifyClient(dtoAcceptedBooking.ClientId, dtoAcceptedBooking.ProviderName, dtoAcceptedBooking.ServiceName, redirectUrl);
        if (clientConnectionIds is null) return dtoAcceptedBooking;
        var clientConnectionId = clientConnectionIds.LastOrDefault();
        if (clientConnectionId is null) return dtoAcceptedBooking;
        Clients.Client(clientConnectionId)?.SendAsync("BookingAccepted", dtoAcceptedBooking);
        return dtoAcceptedBooking;
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

    private void NotifyClient(Guid clientId, string providerName, string serviceName, string redirectUrl)
    {
        var message = $"{providerName} vừa đồng ý dịch vụ {serviceName} của bạn.";
        BackgroundJob.Schedule(() => _notificationService.CreateNotification(clientId, message, redirectUrl), TimeSpan.Zero);
    }
}