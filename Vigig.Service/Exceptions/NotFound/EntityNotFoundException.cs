using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions.NotFound;

public abstract class EntityNotFoundException<T> : ArgumentException,INotFoundException
{
    public readonly string? _customeMessage;
    public override string Message => _customeMessage ?? Message;
    public EntityNotFoundException()
    {
        _customeMessage = $"Entity of type '{typeof(T).Name}' was not found";
    }

    public EntityNotFoundException(object validateValue, object validateProperty)
    {
        var propertyName = typeof(T).GetProperty((string)validateProperty) ??
                           throw new Exception($"Not found property name: {validateProperty}.");
        
        _customeMessage = $"Entity of type '{typeof(T).Name}' with {validateProperty} '{validateValue}' was not found.";
    }

    
}