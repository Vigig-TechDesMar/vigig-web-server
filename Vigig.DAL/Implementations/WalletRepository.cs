using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class WalletRepository : GenericRepository<Wallet>
{
    public WalletRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}