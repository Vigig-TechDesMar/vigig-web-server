namespace Vigig.Service.Models.Response.DashBoard;

public class AdminBookingDashBoardResponse
{
    public int CompletedBooking { get; set; }
    
    public int CancelledByClientBooking { get; set; }
    
    public int CancelledByProviderBooking { get; set; }
    
    public int TimeoutBooking { get; set; }
    
    public int DeclinedBooking { get; set; }
}