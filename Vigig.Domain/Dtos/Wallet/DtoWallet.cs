namespace Vigig.Domain.Dtos.Wallet;

public class DtoWallet
{
    public Guid Id { get; set; }

    public double? Balance { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid ProviderId { get; set; }
}