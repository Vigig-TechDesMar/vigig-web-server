using Microsoft.EntityFrameworkCore;

namespace Vigig.Domain.Interfaces;

public interface IDatabaseModelMapper
{
    void Map(ModelBuilder modelBuilder);
}