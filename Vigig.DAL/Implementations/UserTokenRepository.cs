using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
{
    public UserTokenRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}