using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.Domain.DatabaseMappings;

public class ProviderKPIModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProviderKPI>(entity =>
        {
            entity.ToTable(nameof(ProviderKPI));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Progress);
            entity.Property(e => e.CreatedDate)
                .IsRequired();
            entity.Property(e => e.ProviderId);
            entity.Property(e => e.LeaderBoardId);
            entity.HasOne(e => e.Provider)
                .WithMany(p => p.KPIs)
                .HasForeignKey(e => e.ProviderId);
            entity.HasOne(e => e.LeaderBoard)
                .WithMany(p => p.KPIs)
                .HasForeignKey(e => e.LeaderBoardId);
        });
    }
}
