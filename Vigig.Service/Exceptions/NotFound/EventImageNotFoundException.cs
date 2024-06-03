using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class EventImageNotFoundException : EntityNotFoundException<EventImage>
{
    public EventImageNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}