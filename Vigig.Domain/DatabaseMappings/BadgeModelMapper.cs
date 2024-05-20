using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class BadgeModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Badge>(entity =>
        {
            entity.ToTable("Badge");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.BadgeName).HasMaxLength(450);
            entity.Property(e => e.Benefit).HasMaxLength(450);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken().HasValueGenerator<StringValueGenerator>();

        });
    }
}