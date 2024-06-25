using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.DashBoard;

public class DashBoardRequest
{
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}