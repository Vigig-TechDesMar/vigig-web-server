using Vigig.Common.Constants.Validations;
using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions;

public class InvalidPasswordException : ArgumentException,IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public InvalidPasswordException()
    {
        _customMessage = UserProfileValidation.Password.InvalidPasswordMessage;
    }

    public InvalidPasswordException(string customMessage)
    {
        _customMessage = customMessage;
    }
}