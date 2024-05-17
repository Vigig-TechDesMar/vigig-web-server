using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.AlreadyExist;

public class ServiceCategoryAlreadyExistException : EntityAlreadyExistException<ServiceCategory>
{
    public ServiceCategoryAlreadyExistException(object validateValue, object validateProperty) : base(validateValue, validateProperty)
    {
    }
}