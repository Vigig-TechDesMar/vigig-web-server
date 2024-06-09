namespace Vigig.Domain.Enums;

public enum BookingStatus
{
    Pending = 0,
    Accepted = 1,
    Completed = 2,
    Declined = 3,
    CancelledByProvider = 4,
    CancelledByClient = 5,
    Timeout = 6
}
