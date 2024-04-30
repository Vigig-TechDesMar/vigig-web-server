using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class BuildingModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.ToTable("Building");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BuildingName).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken().HasValueGenerator<StringValueGenerator>();
        });
    }
}