using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class NotificationRepository : GenericRepository<Notification>
{
    public NotificationRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}