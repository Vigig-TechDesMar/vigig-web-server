using Vigig.Domain.Entities;


namespace Vigig.Service.Exceptions.NotFound;

public class ServiceCategoryNotFoundException : EntityNotFoundException<ServiceCategory>
{
    public ServiceCategoryNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}