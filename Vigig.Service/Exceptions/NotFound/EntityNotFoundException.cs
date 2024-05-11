﻿using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions.NotFound;

public abstract class EntityNotFoundException<T> : ArgumentException, IBusinessException 
{
    public readonly string? _customeMessage;
    public override string Message => _customeMessage ?? Message;

    public EntityNotFoundException(string id)
    {
        _customeMessage = $"Entity of type '{typeof(T).Name}' with id '{id}' was not found.";
    }
}