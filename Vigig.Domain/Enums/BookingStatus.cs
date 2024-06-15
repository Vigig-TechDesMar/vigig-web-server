using System.ComponentModel;

namespace Vigig.Domain.Enums;

public enum BookingStatus
{
    [Description(BookingStatusConstant.Pending)]
    Pending = 0,
    [Description(BookingStatusConstant.Accepted)]
    Accepted = 1,
    [Description(BookingStatusConstant.Completed)]
    Completed = 2,
    [Description(BookingStatusConstant.Declined)]
    Declined = 3,
    [Description(BookingStatusConstant.CancelledByProvider)]
    CancelledByProvider = 4,
    [Description(BookingStatusConstant.CancelledByClient)]
    CancelledByClient = 5,
    [Description(BookingStatusConstant.Timeout)]
    Timeout = 6,
    [Description(BookingStatusConstant.Closed)]
    Closed = 7
}

public static class BookingStatusConstant
{
    public const string Pending = "Chưa giải quyết";
    public const string Accepted = "Đã đồng ý";
    public const string Completed = "Đã hoàn thành";
    public const string Declined = "Đã từ chối";
    public const string CancelledByProvider = "Hủy bởi nhà cung cấp";
    public const string CancelledByClient = "Hủy bởi khách hàng";
    public const string Timeout = "Hết thời gian";
    public const string Closed = "Đã kết thúc";
}