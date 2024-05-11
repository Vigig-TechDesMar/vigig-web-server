using Vigig.DAL.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.DAL.Implementations;

public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
{
    public ServiceCategoryRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}