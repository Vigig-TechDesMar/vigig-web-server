using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class BookingRepository : GenericRepository<Booking>
{
    public BookingRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}