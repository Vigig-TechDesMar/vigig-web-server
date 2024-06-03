using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class SubscriptionFeeNotFoundException : EntityNotFoundException<SubscriptionFee>
{
    public SubscriptionFeeNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}