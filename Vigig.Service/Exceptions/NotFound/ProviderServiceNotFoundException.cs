using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class ProviderServiceNotFoundException : EntityNotFoundException<ProviderService>
{
    public ProviderServiceNotFoundException(object validateValue, object validateProperty) : base(validateValue,validateProperty)
    {
    }
}