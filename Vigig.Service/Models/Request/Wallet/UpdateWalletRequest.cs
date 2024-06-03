namespace Vigig.Service.Models.Request.Wallet;

public class UpdateWalletRequest
{
    public Guid Id { get; set; }

    public double Balance { get; set; }

    public bool IsActive { get; set; }
    
    public Guid ProviderId { get; set; }
}