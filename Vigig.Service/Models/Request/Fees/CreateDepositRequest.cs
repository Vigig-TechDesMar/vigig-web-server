using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;
using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Fees;

public class CreateDepositRequest
{
    [Required]
    [MinValue(FeeConstant.MinFee)]
    public double Amount { get; set; }

    [Required] 
    public string PaymentMethod { get; set; } = null!; 

}