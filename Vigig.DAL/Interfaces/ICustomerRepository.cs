using Vigig.Common.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.DAL.Interfaces;

public interface ICustomerRepository : IGenericRepository<Customer>,IAutoRegisterable
{
    
}