using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class VigigRoleModalMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VigigRole>(entity =>
        {
            entity.ToTable("VigigRole");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.NormalizedName).HasMaxLength(255);

            entity.HasMany<VigigUser>().WithMany(u => u.Roles);
        });
    }
}