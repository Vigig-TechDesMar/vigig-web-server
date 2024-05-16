using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class EventRepository : GenericRepository<Event>
{
    public EventRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}