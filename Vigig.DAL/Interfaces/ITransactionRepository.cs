using Vigig.Common.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Interfaces;

public interface ITransactionRepository : IGenericRepository<Transaction>,IAutoRegisterable
{
    
}