using System.Transactions;
using Vigig.DAL.Interfaces;

namespace Vigig.DAL.Implementations;

public class TransactionRepository : GenericRepository<Transaction>
{
    public TransactionRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}