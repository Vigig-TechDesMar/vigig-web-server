using Vigig.Common.Attribute;
using Vigig.Domain.Entities;

namespace Vigig.Service.Interfaces;

[ServiceRegister]

public interface IBookingMessageService
{
    Task<BookingMessage> SendMessage(string token, Guid bookingId, string message);

    Task<IQueryable> LoadAllBookingMessage(string token, Guid bookingId);
}