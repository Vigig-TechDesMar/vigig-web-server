using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class UserNotFoundException : EntityNotFoundException<VigigUser>
{
    public UserNotFoundException(string id) : base(id)
    {
    }
}