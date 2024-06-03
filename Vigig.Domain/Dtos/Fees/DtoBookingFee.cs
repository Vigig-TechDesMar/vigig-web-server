namespace Vigig.Domain.Dtos.Fees;

public class DtoBookingFee
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid BookingId { get; set; }
}