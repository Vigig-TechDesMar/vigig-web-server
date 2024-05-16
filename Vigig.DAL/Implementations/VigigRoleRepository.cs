using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class VigigRoleRepository : GenericRepository<VigigRole>, IVigigRoleRepository
{
    public VigigRoleRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}