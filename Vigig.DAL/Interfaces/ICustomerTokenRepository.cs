using Vigig.Common.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.DAL.Interfaces;

public interface ICustomerTokenRepository : IGenericRepository<CustomerToken>, IAutoRegisterable
{
    
}