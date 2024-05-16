using Vigig.DAL.Interfaces;

using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class GigServiceRepository : GenericRepository<GigService>
{
    public GigServiceRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}