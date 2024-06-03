using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.AlreadyExist;

public class SubscriptionPlanAlreadyExistException: EntityAlreadyExistException<SubscriptionPlan>
{
    public SubscriptionPlanAlreadyExistException(object validateValue, object validateProperty) : base(validateValue,
        validateProperty)
    {
        
    }    
}