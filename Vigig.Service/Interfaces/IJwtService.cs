using Vigig.Common.Attribute;
using Vigig.Common.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IJwtService 
{
    string GenerateAccessToken(VigigUser vigigUser, ICollection<string> roles);

    Task<string> GenerateRefreshToken(Guid customerId);
}