using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class SubscriptionFeeModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriptionFee>(entity =>
        {
            entity.ToTable("SubscriptionFee");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Amount).HasDefaultValueSql("((0))");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Provider).WithMany(p => p.SubscriptionFees)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__Provi__6D0D32F4");

            entity.HasOne(d => d.SubscriptionPlan).WithMany(p => p.SubscriptionFees)
                .HasForeignKey(d => d.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__Subsc__6E01572D");
        });
    }
}