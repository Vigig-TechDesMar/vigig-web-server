using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;
using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Fees;

public class UpdateBookingFeeRequest
{
    public Guid Id { get; set; }

    [Required]
    [MinValue(FeeConstant.MinFee)]
    public double? Amount { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; }
        
    public Guid BookingId { get; set; }
    
}