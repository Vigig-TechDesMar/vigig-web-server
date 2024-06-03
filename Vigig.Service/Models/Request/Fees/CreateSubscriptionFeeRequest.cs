using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;
using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Fees;

public class CreateSubscriptionFeeRequest
{
    [Required]
    [MinValue(FeeConstant.MinFee)]
    public double? Amount { get; set; }
    
    [Required]
    public DateTime? CreatedDate { get; set; }

    public Guid ProviderId { get; set; }

    public Guid SubscriptionPlanId { get; set; }
}