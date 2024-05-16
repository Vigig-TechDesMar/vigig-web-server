using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class ComplaintTypeModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComplaintType>(entity =>
        {
            entity.ToTable("ComplaintType");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e =>  e.Name).HasColumnName("Name");
            entity.Property(e => e.Description).HasColumnName("Description");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });
    }
}