using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class ProviderServiceRepository : GenericRepository<ProviderService>
{
    public ProviderServiceRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}