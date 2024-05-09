using Vigig.Common.Constants.Messages;
using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions;

public class BuildingAlreadyExistException: ArgumentException, IBusinessException
{
    public readonly string? _customMessage;
    public override string Message => _customMessage ?? Message;

    public BuildingAlreadyExistException(string customMessage)
    {
        _customMessage = ExceptionMessage.BuildingMessage.BuildingAlreadyExistMessage(customMessage);
    }
    
    public BuildingAlreadyExistException()
    {
        _customMessage = ExceptionMessage.BuildingMessage.BuildingAlreadyExistMessage(String.Empty);
    }
}