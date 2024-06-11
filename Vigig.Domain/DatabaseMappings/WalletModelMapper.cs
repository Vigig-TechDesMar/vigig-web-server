using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class WalletModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Balance).HasDefaultValueSql("((0))").IsRequired();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.CreatedDate);

            entity.HasOne(d => d.Provider).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Wallet__Provider__74AE54BC");
        });
    }
}