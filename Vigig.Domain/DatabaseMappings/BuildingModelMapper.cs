using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class BuildingModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.ToTable("Building");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.BuildingName).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken().HasValueGenerator<StringValueGenerator>();
        });
    }
}