using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class BadgeRepository : GenericRepository<Badge>, IBadgeRepository
{
    public BadgeRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}