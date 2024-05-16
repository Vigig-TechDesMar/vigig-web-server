using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class LeaderBoardRepository : GenericRepository<LeaderBoard>
{
    public LeaderBoardRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}