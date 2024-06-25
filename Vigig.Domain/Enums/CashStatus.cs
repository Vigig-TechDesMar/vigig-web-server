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

    public const int PendingInt = 0;
    public const int SuccessInt = 1;
    public const int FailInt = 2;
}