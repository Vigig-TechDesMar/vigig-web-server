using System.ComponentModel;

namespace Vigig.Domain.Enums;

public enum ComplaintStatus
{
    [Description(ComplaintStatusConstant.Pending)]
    Pending,
    [Description(ComplaintStatusConstant.Processing)]
    Processing,
    [Description(ComplaintStatusConstant.Resolved)]
    Resolved,
    [Description(ComplaintStatusConstant.Reject)]
    Reject
}

public static class ComplaintStatusConstant
{
    public const string Pending = "Chưa giải quyết";
    public const string Processing = "Đang giải quyết";
    public const string Resolved = "Đã giải quyết";
    public const string Reject = "Đã từ chối giải quyết";
}