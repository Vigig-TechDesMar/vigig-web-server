using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface IDashBoardService
{
    Task<ServiceActionResult> GetBookingByMonth();
}