using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class ClaimedVoucherNotFoundException : EntityNotFoundException<ClaimedVoucher>
{
    public ClaimedVoucherNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}