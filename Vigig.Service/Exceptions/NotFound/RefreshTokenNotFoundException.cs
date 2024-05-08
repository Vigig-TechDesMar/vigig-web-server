using Vigig.Service.Models.Request.Authentication;

namespace Vigig.Service.Exceptions.NotFound;

public class RefreshTokenNotFoundException : EntityNotFoundException<RefreshTokenRequest>
{
    public RefreshTokenNotFoundException(string id) : base(id)
    {
    }
}