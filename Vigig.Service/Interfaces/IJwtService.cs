using Vigig.Common.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.Service.Interfaces;

public interface IJwtService : IAutoRegisterable
{
    string GenerateAccessToken(VigigUser vigigUser, ICollection<VigigRole> roles);

    Task<string> GenerateRefreshToken(Guid customerId);
}