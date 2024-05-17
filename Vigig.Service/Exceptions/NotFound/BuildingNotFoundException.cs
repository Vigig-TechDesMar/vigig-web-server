using Vigig.Common.Exceptions;
using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class BuildingNotFoundException : EntityNotFoundException<Building>
{
    public BuildingNotFoundException(object id) : base(id)
    {
    }
}