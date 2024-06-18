using Vigig.Common.Constants.Messages;

namespace Vigig.Common.Exceptions;

public class MissingGoogleConfiguration: ArgumentException, IBusinessException
{
    private readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingGoogleConfiguration(string customMessage)
    {
        _customMessage = customMessage;
    }

    public MissingGoogleConfiguration()
    {
        _customMessage = ExceptionMessage.MissingGoogleConfiguration;
    }
}