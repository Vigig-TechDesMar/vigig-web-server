using Vigig.Common.Constants.Messages;
using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions;

public class UserAlreadyExistException : ArgumentException, IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public UserAlreadyExistException(string customMessage)
    {
        _customMessage = ExceptionMessage.AuthenticationMessage.UserAlreadyExistMessage(customMessage);
    }
    
    public UserAlreadyExistException()
    {
        _customMessage = ExceptionMessage.AuthenticationMessage.UserAlreadyExistMessage(String.Empty);
    }
}