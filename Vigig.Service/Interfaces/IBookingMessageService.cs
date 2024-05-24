using Vigig.Common.Attribute;
using Vigig.Domain.Entities;

namespace Vigig.Service.Interfaces;

[ServiceRegister]

public interface IBookingMessageService
{
    Task SendMessage(string token, Guid bookingId, string message);
}