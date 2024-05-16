using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class PopUpRepository : GenericRepository<PopUp>, IPopUpRepository
{
    public PopUpRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}