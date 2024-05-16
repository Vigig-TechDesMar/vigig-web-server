using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class ProviderServiceRepository : GenericRepository<ProviderService>, IProviderServiceRepository
{
    public ProviderServiceRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}