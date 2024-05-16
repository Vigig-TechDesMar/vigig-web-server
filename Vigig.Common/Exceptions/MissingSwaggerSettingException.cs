using Vigig.Common.Constants.Messages;

namespace Vigig.Common.Exceptions;

public class MissingSwaggerSettingException: ArgumentException, IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingSwaggerSettingException(string customMessage)
    {
        _customMessage = customMessage;
    }

    public MissingSwaggerSettingException()
    {
        _customMessage = ExceptionMessage.MissingCustomSwaggerConfiguration;
    }
}