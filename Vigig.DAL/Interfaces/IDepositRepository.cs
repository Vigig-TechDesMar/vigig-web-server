using Vigig.Common.Attribute;
using Vigig.Common.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Interfaces;
[ServiceRegister]
public interface IDepositRepository  : IGenericRepository<Deposit>
{
    
}