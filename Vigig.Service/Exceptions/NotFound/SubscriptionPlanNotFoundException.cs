using Vigig.Common.Exceptions;
using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class SubscriptionPlanNotFoundException: EntityNotFoundException<SubscriptionPlan>
{
    public SubscriptionPlanNotFoundException(object id) : base(id)
    {
        
    }
}