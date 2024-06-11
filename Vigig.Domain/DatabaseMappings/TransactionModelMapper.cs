using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class TransactionModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Amount).HasDefaultValueSql("((0))").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.BookingFee).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BookingFeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);

            entity.HasOne(d => d.Deposit).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DepositId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);

            entity.HasOne(d => d.SubscriptionFee).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.SubscriptionFeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired();
        });
    }
}