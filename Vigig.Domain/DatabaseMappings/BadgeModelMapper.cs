using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class BadgeModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Badge>(entity =>
        {
            entity.ToTable("Badge");
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BadgeName).HasMaxLength(450);
            entity.Property(e => e.Benefit).HasMaxLength(450);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });
    }
}