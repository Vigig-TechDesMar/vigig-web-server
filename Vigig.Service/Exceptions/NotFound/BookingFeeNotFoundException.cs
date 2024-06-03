using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class BookingFeeNotFoundException : EntityNotFoundException<BookingFee>
{
    public BookingFeeNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}