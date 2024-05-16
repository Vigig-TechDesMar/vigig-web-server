using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class ProviderKPIRepository : GenericRepository<ProviderKPI>
{
    public ProviderKPIRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}