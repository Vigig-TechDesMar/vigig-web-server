using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class EventNotFoundException : EntityNotFoundException<Event>
{
    public EventNotFoundException(object id) : base(id)
    {
    }
}