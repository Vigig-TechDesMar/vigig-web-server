using Vigig.Common.Constants;
using Vigig.Common.Constants.Messages;

namespace Vigig.Common.Exceptions;

public class MissingConnectionStringException : ArgumentNullException
{
    private string? CustomMessage { get; set; }
    public override string Message => CustomMessage ?? Message;

    public MissingConnectionStringException(string customMessage)
    {
        CustomMessage = customMessage;
    }

    public MissingConnectionStringException()
    {
        CustomMessage = ExceptionMessage.MissingConnectionString;
    }
    
}