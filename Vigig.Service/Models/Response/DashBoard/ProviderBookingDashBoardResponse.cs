namespace Vigig.Service.Models.Response.DashBoard;

public class ProviderBookingDashBoardResponse
{
    public int CompletedBooking { get; set; }
    
    public int CancelledBooking { get; set; }
    
    public int TimeoutBooking { get; set; }
    
    public int DeclinedBooking { get; set; }
}