namespace Vigig.Service.Models.Request.Wallet;

public class CreateWalletRequest
{
    public double Balance { get; set; }

    public bool IsActive { get; set; }
    
    public Guid ProviderId { get; set; }
}