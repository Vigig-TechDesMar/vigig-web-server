using Vigig.DAL.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.DAL.Implementations;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository 
{
    public CustomerRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}