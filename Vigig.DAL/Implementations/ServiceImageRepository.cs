using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class ServiceImageRepository : GenericRepository<ServiceImage>
{
    public ServiceImageRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}