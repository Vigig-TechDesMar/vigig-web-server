using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class NotificationTypeRepository : GenericRepository<NotificationType>
{
    public NotificationTypeRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}