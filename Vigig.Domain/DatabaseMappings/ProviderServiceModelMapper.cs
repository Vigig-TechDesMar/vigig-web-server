using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class ProviderServiceModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProviderService>(entity =>
        {
            entity.ToTable("ProviderService");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsAvailable).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsVisible).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderServices)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderS__Provi__66603565");

            entity.HasOne(d => d.Service).WithMany(p => p.ProviderServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderS__Servi__6754599E");
        });
    }
}