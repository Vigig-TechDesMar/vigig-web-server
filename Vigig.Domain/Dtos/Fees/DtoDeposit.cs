using Vigig.Domain.Enums;

namespace Vigig.Domain.Dtos.Fees;

public class DtoDeposit
{
    public Guid Id { get; set; }

    public double Amount { get; set; }

    public DateTime CreatedDate { get; set; }

    public required string PaymentMethod { get; set; } 

    public Guid ProviderId { get; set; }
    
    public CashStatus Status { get; set; }
    
    public required string CheckoutUrl { get; set; }
}