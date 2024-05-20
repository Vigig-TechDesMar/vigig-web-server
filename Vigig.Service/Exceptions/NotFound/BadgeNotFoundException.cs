using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class BadgeNotFoundException : EntityNotFoundException<Badge>
{
    public BadgeNotFoundException(object id) : base(id)
    {
    }
}