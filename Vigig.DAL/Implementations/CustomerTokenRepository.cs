using Vigig.DAL.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.DAL.Implementations;

public class CustomerTokenRepository : GenericRepository<CustomerToken>, ICustomerTokenRepository
{
    public CustomerTokenRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}