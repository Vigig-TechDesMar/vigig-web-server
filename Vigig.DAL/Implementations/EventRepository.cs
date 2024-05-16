using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class EventRepository : GenericRepository<Event>, IEventRepository
{
    public EventRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}