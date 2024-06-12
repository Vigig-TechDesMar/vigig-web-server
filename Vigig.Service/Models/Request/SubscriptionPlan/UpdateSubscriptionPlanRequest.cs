using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;

namespace Vigig.Service.Models.Request.SubscriptionPlan;

public class UpdateSubscriptionPlanRequest
{
    public Guid Id { get; set; }

    public string? Description { get; set; } = string.Empty;

    [Required]
    [MinValue(1)]
    public double? DurationType { get; set; }

    [Required]
    [MinValue(0)]
    public double? Price { get; set; }
}