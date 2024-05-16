using System.Transactions;
using Vigig.DAL.Interfaces;
using Transaction = Vigig.Domain.Entities.Transaction;

namespace Vigig.DAL.Implementations;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}