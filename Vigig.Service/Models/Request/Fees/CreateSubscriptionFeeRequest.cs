using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;
using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Fees;

public class CreateSubscriptionFeeRequest
{

    [Required]
    public Guid SubscriptionPlanId { get; set; }
}