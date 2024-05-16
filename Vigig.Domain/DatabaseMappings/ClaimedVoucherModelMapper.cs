using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.Domain.DatabaseMappings;

public class ClaimedVoucherModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClaimedVoucher>(entity =>
        {
            entity.ToTable(nameof(ClaimedVoucher));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.EventTitle);
            entity.Property(e => e.StartDate);
            entity.Property(e => e.EndDate);
            entity.Property(e => e.Field);
            entity.HasOne(e => e.Voucher)
                .WithMany(v => v.ClaimedVouchers)
                .HasForeignKey(e => e.VoucherId);
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.ClaimedVouchers)
                .HasForeignKey(e => e.CustomerId);
        });
    }
}
