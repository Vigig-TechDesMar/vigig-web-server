using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class BookingMessageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingMessage>(entity =>
        {
            entity.ToTable("BookingMessage");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.SenderName).HasMaxLength(450);
            entity.Property(e => e.SentAt).HasColumnType("datetime");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingMessages)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingMe__Booki__02FC7413");
        });
    }
}