using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class GigServiceNotFoundException : EntityNotFoundException<GigService>
{
    public GigServiceNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}