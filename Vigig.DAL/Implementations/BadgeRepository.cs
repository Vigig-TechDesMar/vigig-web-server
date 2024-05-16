using Vigig.DAL.Interfaces;

namespace Vigig.DAL.Implementations;

public class BadgeRepository : GenericRepository<BadgeRepository>
{
    public BadgeRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}