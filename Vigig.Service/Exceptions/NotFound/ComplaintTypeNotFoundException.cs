using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class ComplaintTypeNotFoundException : EntityNotFoundException<ComplaintType>
{
    public ComplaintTypeNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}