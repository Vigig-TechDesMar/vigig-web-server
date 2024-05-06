using Vigig.Common.Constants.Messages;

namespace Vigig.Common.Exceptions;

public class MissingJwtSettingsException : ArgumentException, IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingJwtSettingsException(string customMessage)
    {
        _customMessage = customMessage;
    }

    public MissingJwtSettingsException()
    {
        _customMessage = ExceptionMessage.MissingJwtSettingsConfiguration;
    }
}