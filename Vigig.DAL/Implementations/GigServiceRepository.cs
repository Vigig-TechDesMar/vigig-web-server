using Vigig.DAL.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.DAL.Implementations;

public class GigServiceRepository : GenericRepository<GigService>, IGigServiceRepository
{
    public GigServiceRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}