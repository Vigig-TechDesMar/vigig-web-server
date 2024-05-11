using Vigig.DAL.Interfaces;
<<<<<<< HEAD
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class ServiceCategoryRepository : GenericRepository<ServiceCategory>
=======
using Vigig.Domain.Models;

namespace Vigig.DAL.Implementations;

public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
>>>>>>> a13f8b9 ([ServiceCategory][Hai] add service category mangament service)
{
    public ServiceCategoryRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}