using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class DepositNotFoundException : EntityNotFoundException<Deposit>
{
    public DepositNotFoundException(object id) : base(id)
    {
        
    }
}