using Vigig.Common.Constants.Validations;
using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions;

public class PasswordTooWeakException : ArgumentException,IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;
    
    public PasswordTooWeakException(string customMessage)
    {
        _customMessage = customMessage;
    }
    
    public PasswordTooWeakException()
    {
        _customMessage = UserProfileValidation.Password.NotMatchedPatternMessage;
    }
}