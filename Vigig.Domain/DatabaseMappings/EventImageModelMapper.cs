using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.Domain.DatabaseMappings;

public class EventImageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventImage>(entity =>
        {
            entity.ToTable(nameof(EventImage));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.EventId);
            entity.Property(e => e.ImageUrl)
                .IsRequired();
            entity.Property(e => e.StartDate)
                .IsRequired();
            entity.Property(e => e.EndDate)
                .IsRequired();
            entity.HasOne(e => e.Event)
                .WithMany(e => e.EventImages)
                .HasForeignKey(e => e.EventId);
        });
    }
}
