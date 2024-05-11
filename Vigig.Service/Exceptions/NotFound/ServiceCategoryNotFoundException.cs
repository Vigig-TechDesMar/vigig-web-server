using Vigig.Domain.Entities;
using Vigig.Domain.Models;

namespace Vigig.Service.Exceptions.NotFound;

public class ServiceCategoryNotFoundException : EntityNotFoundException<ServiceCategory>
{
    public ServiceCategoryNotFoundException(string id) : base(id)
    {
    }
}