using Vigig.Common.Exceptions;
using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class SubscriptionPlanNotFoundException: EntityNotFoundException<SubscriptionPlan>
{
    public SubscriptionPlanNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}