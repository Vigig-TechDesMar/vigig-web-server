using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Notification;
using Vigig.Domain.Entities;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Notification;

namespace Vigig.Service.Implementations;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationTypeRepository _notificationTypeRepository;
    private readonly IBookingRepository _bookingRepository;

    private readonly IMapper _mapper;
    // private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork, INotificationTypeRepository notificationTypeRepository, IBookingRepository bookingRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        // _currentUser = currentUser;
        _unitOfWork = unitOfWork;
        _notificationTypeRepository = notificationTypeRepository;
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }
    
    public Task<ServiceActionResult> GetOwnNotifications()
    {
        throw new NotImplementedException();
    }

    public async Task CreateBookingNotification(CreateBookingNotificationRequest request)
    {
        var booking = (await _bookingRepository.FindAsync(x => x.Id == request.BookingId)).Include(x => x.ProviderService).FirstOrDefault()
            ?? throw new BookingNotFoundException(request.BookingId, nameof(Booking.Id));
        var notificationType =
            (await _notificationTypeRepository.FindAsync(x => x.Name == NotificationTypeConstants.BookingNotification))
            .FirstOrDefault() ?? throw new NotificationTypeNotFoundException(NotificationTypeConstants.BookingNotification, nameof(NotificationType.Name));
        var notification = new Notification()
        {
            Content = request.Content ?? string.Empty,
            CreatedAt = DateTime.Now,
            UserId = booking.ProviderService.ProviderId,
            NotificationTypeId = notificationType.Id
        };
        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IQueryable<DtoNotification>> RetrieveProviderNotification(Guid providerId)
    {
        var notifications = await _notificationRepository.FindAsync(x => x.IsActive && x.UserId == providerId);
        return _mapper.ProjectTo<DtoNotification>(notifications);
    }

    public Task<ServiceActionResult> CreateNotifications(CreateEventNotificationRequest request, string target)
    {
        throw new NotImplementedException();
    }
}