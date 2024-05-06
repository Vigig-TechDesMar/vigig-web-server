using Vigig.Common.Exceptions;
using Vigig.Domain.Models;

namespace Vigig.Service.Exceptions.NotFound;

public class BuildingNotFoundException : EntityNotFoundException<Building>
{
    public BuildingNotFoundException(string id) : base(id)
    {
    }
}