using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class DepositRepository : GenericRepository<Deposit>
{
    public DepositRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}