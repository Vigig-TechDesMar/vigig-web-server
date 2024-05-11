using Vigig.Domain.Models;

namespace Vigig.Service.Exceptions.AlreadyExist;

public class ServiceCategoryAlreadyExistException : EntityAlreadyExistException<ServiceCategory>
{
    public ServiceCategoryAlreadyExistException(string validateValue, string validateProperty) : base(validateValue, validateProperty)
    {
    }
}