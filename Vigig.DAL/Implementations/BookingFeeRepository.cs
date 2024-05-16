using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class BookingFeeRepository : GenericRepository<BookingFee>,IBookingFeeRepository
{
    public BookingFeeRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
} 