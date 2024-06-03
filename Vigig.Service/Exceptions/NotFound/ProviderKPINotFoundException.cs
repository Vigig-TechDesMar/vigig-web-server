using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class ProviderKPINotFoundException : EntityNotFoundException<ProviderKPI>
{
    public ProviderKPINotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}