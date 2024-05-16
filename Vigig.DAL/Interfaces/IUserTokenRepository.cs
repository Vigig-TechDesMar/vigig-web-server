using Vigig.Common.Attribute;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Interfaces;
[ServiceRegister]
public interface IUserTokenRepository : IGenericRepository<UserToken>
{
    
}