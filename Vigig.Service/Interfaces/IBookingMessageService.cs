using Vigig.Common.Attribute;
using Vigig.Domain.Dtos.Booking;
using Vigig.Domain.Entities;

namespace Vigig.Service.Interfaces;

[ServiceRegister]

public interface IBookingMessageService
{
    Task<DtoBookingMessage> SendMessage(string token, Guid bookingId, string message);

    Task<IQueryable<DtoBookingMessage>> LoadAllBookingMessage(string token, Guid bookingId);
}