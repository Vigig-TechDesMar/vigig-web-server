using Vigig.DAL.Interfaces;
<<<<<<< HEAD
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class GigServiceRepository : GenericRepository<GigService>
=======
using Vigig.Domain.Models;

namespace Vigig.DAL.Implementations;

public class GigServiceRepository : GenericRepository<GigService>, IGigServiceRepository
>>>>>>> a13f8b9 ([ServiceCategory][Hai] add service category mangament service)
{
    public GigServiceRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}