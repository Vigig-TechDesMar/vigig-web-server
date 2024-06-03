namespace Vigig.Domain.Dtos.Wallet;

public class DtoTransaction
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public int Status { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid WalletId { get; set; }

    public Guid DepositId { get; set; }

    public Guid BookingFeeId { get; set; }

    public Guid SubscriptionFeeId { get; set; }
}