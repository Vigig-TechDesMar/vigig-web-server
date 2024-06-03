using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class LeaderBoardNotFoundException : EntityNotFoundException<LeaderBoard>
{
    public LeaderBoardNotFoundException(object id) : base(id)
    {
    }
}