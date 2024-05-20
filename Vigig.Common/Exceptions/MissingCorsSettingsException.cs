using Vigig.Common.Constants.Messages;

namespace Vigig.Common.Exceptions;

public class MissingCorsSettingsException: ArgumentException, IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingCorsSettingsException(string customMessage)
    {
        _customMessage = customMessage;
    }

    public MissingCorsSettingsException()
    {
        _customMessage = ExceptionMessage.MissingCorsSettingsConfiguration;
    }
}