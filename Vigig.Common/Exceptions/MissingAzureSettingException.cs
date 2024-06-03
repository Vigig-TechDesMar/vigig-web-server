using Vigig.Common.Constants.Messages;

namespace Vigig.Common.Exceptions;

public class MissingAzureSettingException: ArgumentException, IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingAzureSettingException(string customMessage)
    {
        _customMessage = customMessage;
    }

    public MissingAzureSettingException()
    {
        _customMessage = ExceptionMessage.MissingAzureConfiguration;
    }
}