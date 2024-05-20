using Vigig.Common.Constants.Messages;
using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions;

public class InvalidTokenException: ArgumentException,IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public InvalidTokenException()
    {
        _customMessage = ExceptionMessage.AuthenticationMessage.InvalidTokenMessage;
    }

    public InvalidTokenException(string customMessage)
    {
        _customMessage = customMessage;
    }
}