using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class BookingMessageRepository : GenericRepository<BookingMessage>,IBookingMessageRepository
{
    public BookingMessageRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}