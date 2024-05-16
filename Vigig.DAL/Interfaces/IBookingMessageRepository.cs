using Vigig.Common.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Interfaces;

public interface IBookingMessageRepository : IGenericRepository<BookingMessage>, IAutoRegisterable
{
    
}