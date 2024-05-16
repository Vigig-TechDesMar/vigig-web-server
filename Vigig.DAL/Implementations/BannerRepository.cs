using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class BannerRepository : GenericRepository<Banner>,IBannerRepository
{
    public BannerRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}