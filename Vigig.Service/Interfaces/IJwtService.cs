using Vigig.Common.Attribute;
using Vigig.Common.Interfaces;
using Vigig.Domain.Entities;
using Vigig.Service.Models.Common;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IJwtService 
{
    string GenerateAccessToken(VigigUser vigigUser, ICollection<string> roles);

    Task<string> GenerateRefreshToken(Guid customerId);

    bool IsValidToken(string token);

    object? GetTokenClaim(string token, string claimName);

    object GetSubjectClaim(string token);

    object GetRoleClaim(string token);

    AuthModel GetAuthModel(string token);
}