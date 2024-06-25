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
            entity.Property(e => e.ImageUrl)
                .IsRequired();
            entity.HasOne(e => e.Banner)
                .WithMany(e => e.EventImages)
                .HasForeignKey(e => e.BannerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);
            entity.HasOne(e => e.PopUp)
                .WithMany(e => e.EventImages)
                .HasForeignKey(e => e.PopUpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);
        }); 
    }
}
