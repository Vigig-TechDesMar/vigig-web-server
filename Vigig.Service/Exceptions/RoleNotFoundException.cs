using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions;

public class RoleNotFoundException : ArgumentException, IBadRequestException
{
    public readonly string? _customeMessage;
    public override string Message => _customeMessage ?? Message;

    public RoleNotFoundException(string role)
    {
        _customeMessage = $"Role {role} is not available in our system.";
    }
}