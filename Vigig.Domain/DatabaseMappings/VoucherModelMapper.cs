using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.Domain.DatabaseMappings;

public class VoucherModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable(nameof(Voucher));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Content);
            entity.Property(e => e.Percentage);
            entity.Property(e => e.Limit);
            entity.Property(e => e.Quantity);
            entity.Property(e => e.StartDate);
            entity.Property(e => e.EndDate);
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))");
            entity.HasOne(e => e.Event)
                .WithMany(e => e.Vouchers)
                .HasForeignKey(e => e.EventId);
        });
    }
}
