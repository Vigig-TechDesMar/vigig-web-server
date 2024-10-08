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
            entity.HasKey(e => new{e.VoucherId,e.CustomerId});
            entity.Property(e => e.EventTitle);
            entity.Property(e => e.UsedDate);
            entity.Property(e => e.IsUsed).HasDefaultValueSql("((0))");
            entity.HasOne(e => e.Voucher)
                .WithMany(v => v.ClaimedVouchers)
                .HasForeignKey(e => e.VoucherId);
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.ClaimedVouchers)
                .HasForeignKey(e => e.CustomerId);
        });
    }
}
