using Vigig.Domain.Models;

namespace Vigig.Service.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(Customer customer);

    string GenerateRefreshToken(Guid customerId);
}