using System.ComponentModel;

namespace Vigig.Domain.Enums;

public enum CashStatus
{
    [Description(CashStatusConstant.Pending)]
    Pending, 
    [Description(CashStatusConstant.Success)]
    Success, 
    [Description(CashStatusConstant.Fail)]
    Fail
}
public static class CashStatusConstant
{
    public const string Pending = "Chưa giải quyết";
    public const string Success = "Thành công";
    public const string Fail = "Thất bại";
}