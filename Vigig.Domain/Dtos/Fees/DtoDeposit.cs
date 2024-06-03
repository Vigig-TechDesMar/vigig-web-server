namespace Vigig.Domain.Dtos.Fees;

public class DtoDeposit
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime MadeDate { get; set; }

    public required string PaymentMethod { get; set; } 

    public Guid ProviderId { get; set; }
}