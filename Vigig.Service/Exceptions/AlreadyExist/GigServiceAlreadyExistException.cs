using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.AlreadyExist;

public class GigServiceAlreadyExistException : EntityAlreadyExistException<GigService>
{
    public GigServiceAlreadyExistException(object validateValue, object validateProperty) : base(validateValue, validateProperty)
    {
    }
}