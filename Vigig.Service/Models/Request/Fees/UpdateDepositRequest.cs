using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;
using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Fees;

public class UpdateDepositRequest
{
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreateDate { get; set; }

    [Required] 
    public string PaymentMethod { get; set; } = null!; 

    public Guid ProviderId { get; set; }
}