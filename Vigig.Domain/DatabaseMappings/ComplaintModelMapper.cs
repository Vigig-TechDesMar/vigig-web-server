using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class ComplaintModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(e => e.Booking)
                .WithMany(e => e.Complaints)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(e => e.ComplaintType)
                .WithMany(e => e.Complaints)
                .HasForeignKey(e => e.ComplaintTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}