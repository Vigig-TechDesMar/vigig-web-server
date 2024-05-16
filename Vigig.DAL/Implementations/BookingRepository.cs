using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class BookingRepository : GenericRepository<Booking>,IBookingRepository
{
    public BookingRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}