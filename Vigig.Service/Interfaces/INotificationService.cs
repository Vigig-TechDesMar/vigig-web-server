﻿using Vigig.Common.Attribute;
using Vigig.Domain.Dtos.Notification;
using Vigig.Service.Constants;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Notification;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface INotificationService
{
    Task<ServiceActionResult> GetOwnNotifications();
    Task CreateBookingNotification(CreateBookingNotificationRequest request);
    Task<IQueryable<DtoNotification>> RetrieveProviderNotification(Guid providerId);
    Task<ServiceActionResult> CreateNotifications(CreateEventNotificationRequest request, string target);
}