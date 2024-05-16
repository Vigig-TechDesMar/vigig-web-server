using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.AlreadyExist;

public class GigServiceAlreadyExistException : EntityAlreadyExistException<GigService>
{
    public GigServiceAlreadyExistException(string validateValue, string validateProperty) : base(validateValue, validateProperty)
    {
    }
}