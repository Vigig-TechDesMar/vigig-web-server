using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;
using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Fees;

public class CreateBookingFeeRequest
{
    [Required]
    [MinValue(FeeConstant.MinFee)]
    public double Amount { get; set; }
        
    public Guid BookingId { get; set; }
}