using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class UserNotFoundException : EntityNotFoundException<VigigUser>
{
    public UserNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }

}