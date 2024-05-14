using Vigig.Common.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.DAL.Interfaces;

public interface IUserTokenRepository : IGenericRepository<UserToken>, IAutoRegisterable
{
    
}