using Vigig.Common.Constants.Messages;

namespace Vigig.Common.Exceptions;

public class MissingPayOSSetting: ArgumentException, IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public MissingPayOSSetting(string customMessage)
    {
        _customMessage = customMessage;
    }

    public MissingPayOSSetting()
    {
        _customMessage = ExceptionMessage.MissingPayOSConfiguration;
    }
}