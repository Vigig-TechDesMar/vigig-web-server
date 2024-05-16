using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class BuildingRepository : GenericRepository<Building>, IBuildingRepository
{
    public BuildingRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}