using Vigig.Common.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Service.Interfaces;

public interface IJwtService : IAutoRegisterable
{
    string GenerateAccessToken(VigigUser vigigUser);

    Task<string> GenerateRefreshToken(Guid customerId);
}