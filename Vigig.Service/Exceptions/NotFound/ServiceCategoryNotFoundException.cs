using Vigig.Domain.Models;

namespace Vigig.Service.Exceptions.NotFound;

public class ServiceCategoryNotFoundException : EntityNotFoundException<ServiceCategory>
{
    public ServiceCategoryNotFoundException(object id) : base(id)
    {
    }
}