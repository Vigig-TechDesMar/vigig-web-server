using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions.NotFound;

public abstract class EntityNotFoundException<T> : ArgumentException,INotFoundException
{
    public readonly string? _customeMessage;
    public override string Message => _customeMessage ?? Message;
    public EntityNotFoundException()
    {
        _customeMessage = $"'{typeof(T).Name}' không tồn tại";
    }

    public EntityNotFoundException(object validateValue, object validateProperty)
    {
        var propertyName = typeof(T).GetProperty((string)validateProperty) ??
                           throw new Exception($"Not found property name: {validateProperty}.");
        
        _customeMessage = $"'{typeof(T).Name}' với {validateProperty} '{validateValue}' không tồn tại.";
    }

    
}