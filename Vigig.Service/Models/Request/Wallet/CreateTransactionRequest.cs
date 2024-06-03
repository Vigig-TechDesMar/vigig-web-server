using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;
using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Wallet;

public class CreateTransactionRequest
{
    [Required]
    [MinValue(FeeConstant.MinFee)]
    public double? Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public int Status { get; set; }

    public Guid WalletId { get; set; }

    public Guid DepositId { get; set; }

    public Guid BookingFeeId { get; set; }

    public Guid SubscriptionFeeId { get; set; }
}