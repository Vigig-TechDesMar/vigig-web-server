using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class DepositModelMapper :IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.ToTable("Deposit");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Amount).HasDefaultValueSql("((0))");
            entity.Property(e => e.MadeDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(255);

            entity.HasOne(d => d.Provider).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}