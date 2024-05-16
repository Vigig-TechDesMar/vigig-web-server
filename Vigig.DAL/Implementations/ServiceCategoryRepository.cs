using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;
namespace Vigig.DAL.Implementations;
public class ServiceCategoryRepository : GenericRepository<ServiceCategory>

{
    public ServiceCategoryRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}