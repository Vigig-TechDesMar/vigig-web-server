using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class WalletNotFoundException : EntityNotFoundException<Wallet>
{
    public WalletNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}