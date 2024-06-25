using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.DashBoard;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IDashBoardService
{
    Task<ServiceActionResult> ProviderGetBookings(DashBoardRequest request, string tok);
    
    Task<ServiceActionResult> ProviderGetTotalCashFlow(DashBoardRequest request, string tok);
    
    Task<ServiceActionResult> ProviderGetComplaints(DashBoardRequest request, string tok);
    
    Task<ServiceActionResult> ProviderGetLeaderBoard(DashBoardRequest request, string tok);
    
    Task<ServiceActionResult> ProviderGetTransaction(DashBoardRequest request, string tok);
    
    Task<ServiceActionResult> AdminGetBookings(DashBoardRequest request, string tok);

    Task<ServiceActionResult> AdminGetComplaints(DashBoardRequest request, string tok);
    
    Task<ServiceActionResult> AdminGetNewUsers(DashBoardRequest request, string tok);

    Task<ServiceActionResult> AdminGetTotalRevenue(DashBoardRequest request, string tok);

    Task<ServiceActionResult> AdminGetEventsAndChildren(DashBoardRequest request, string tok);

    Task<ServiceActionResult> AdminGetVoucher(DashBoardRequest request, string tok);
    
    Task<ServiceActionResult> AdminGetTransaction(DashBoardRequest request, string tok);
}