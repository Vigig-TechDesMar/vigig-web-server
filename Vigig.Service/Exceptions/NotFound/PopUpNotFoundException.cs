using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class PopUpNotFoundException : EntityNotFoundException<PopUp>
{
    public PopUpNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}