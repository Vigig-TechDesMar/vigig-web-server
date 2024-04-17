using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class ProviderModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provider>(entity =>
        {
            entity.ToTable("Provider");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(450);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EmailConfirmed).HasDefaultValueSql("((0))");
            entity.Property(e => e.ExpirationPlanDate).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(450);
            entity.Property(e => e.Gender).HasMaxLength(63);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.NormalizedEmail).HasMaxLength(255);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasOne(d => d.Badge).WithMany(p => p.Providers)
                .HasForeignKey(d => d.BadgeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Provider__BadgeI__6FE99F9F");

            entity.HasOne(d => d.Building).WithMany(p => p.Providers)
                .HasForeignKey(d => d.BuildingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Provider__Buildi__6EF57B66");
        });
    }
}