using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class SubscriptionPlanRepository : GenericRepository<SubscriptionPlan>
{
    public SubscriptionPlanRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}