using System.Reflection;
using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions.AlreadyExist;

public abstract class EntityAlreadyExistException<T>: ArgumentException,IBusinessException 
{
    public readonly string? _customeMessage;
    
    public override string Message => _customeMessage ?? Message;

    public EntityAlreadyExistException(object validateValue, object validateProperty)
    {
        var propertyName = typeof(T).GetProperty((string)validateProperty) ??
                           throw new Exception($"Not found property name: {validateProperty}.");
        
        _customeMessage = $"Entity of type '{typeof(T).Name}' with {validateProperty} '{validateValue}' already exist.";
    }
}