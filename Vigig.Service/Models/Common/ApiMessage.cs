using Vigig.Service.Enums;

namespace Vigig.Service.Models.Common;

public class ApiMessage
{
    public ApiMessageType Type { get; set; }
    public string Content { get; set; }

    public ApiMessage()
    {
        Content = string.Empty;
    }

    public ApiMessage(string content, ApiMessageType messageType = ApiMessageType.Info)
    {
        Content = content;
        Type = messageType;
    }
}