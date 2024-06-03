using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class TransactionNotFoundException : EntityNotFoundException<Transaction>
{
    public TransactionNotFoundException(object id) : base(id)
    {
        
    }
}