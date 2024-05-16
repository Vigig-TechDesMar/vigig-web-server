using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class ServiceCategoryModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.ToTable("ServiceCategory");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).HasMaxLength(450);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });
    }
}