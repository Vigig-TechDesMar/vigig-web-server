using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class BannerRepository : GenericRepository<Banner>
{
    public BannerRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}