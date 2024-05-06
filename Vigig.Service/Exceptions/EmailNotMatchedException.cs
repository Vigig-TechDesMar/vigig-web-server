using Vigig.Common.Constants.Validations;
using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions;

public class EmailNotMatchedException : ArgumentException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public EmailNotMatchedException()
    {
        _customMessage = UserProfileValidation.Email.NotMatchedPatternMessage;
    }

    public EmailNotMatchedException(string customMessage)
    {
        _customMessage = customMessage;
    }
}