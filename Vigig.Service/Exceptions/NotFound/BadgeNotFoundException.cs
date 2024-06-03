using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class BadgeNotFoundException : EntityNotFoundException<Badge>
{
    public BadgeNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}