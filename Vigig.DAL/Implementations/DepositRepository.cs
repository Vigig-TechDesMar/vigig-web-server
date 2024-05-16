using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class DepositRepository : GenericRepository<Deposit>,IDepositRepository
{
    public DepositRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}