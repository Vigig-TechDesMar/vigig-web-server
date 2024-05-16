using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class LeaderBoardRepository : GenericRepository<LeaderBoard>, ILeaderBoardRepository
{
    public LeaderBoardRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}