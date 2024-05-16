using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class SubscriptionFeeRepository : GenericRepository<SubscriptionFee>
{
    public SubscriptionFeeRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}