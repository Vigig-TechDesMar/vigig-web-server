using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class NotificationTypeRepository : GenericRepository<NotificationType>, INotificationTypeRepository
{
    public NotificationTypeRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}