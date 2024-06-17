using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class NotificationTypeNotFoundException : EntityNotFoundException<NotificationType>
{
    public NotificationTypeNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}