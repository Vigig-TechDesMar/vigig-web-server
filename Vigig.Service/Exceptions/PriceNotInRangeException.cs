using Vigig.Common.Exceptions;

namespace Vigig.Service.Exceptions;

public class PriceNotInRangeException: ArgumentException, IBadRequestException
{
    public readonly string? _customeMessage;
    public override string Message => _customeMessage ?? Message;

    public PriceNotInRangeException(object serviceName)
    {
        // _customeMessage = $"The sticker price should be in range of the service {nameof(serviceName)}.";
        _customeMessage = $"Giá niêm yết nên nằm trong khoảng quy định của {nameof(serviceName)}.";
    }
}