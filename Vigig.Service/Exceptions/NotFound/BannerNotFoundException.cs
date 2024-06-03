using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class BannerNotFoundException : EntityNotFoundException<Banner>
{
    public BannerNotFoundException(object id) : base(id)
    {
    }
}