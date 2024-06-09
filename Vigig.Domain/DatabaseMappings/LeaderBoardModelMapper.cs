using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.Domain.DatabaseMappings;

public class LeaderBoardModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LeaderBoard>(entity =>
        {
            entity.ToTable(nameof(LeaderBoard));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .IsRequired();
            entity.Property(e => e.StartDate);
            entity.Property(e => e.EndDate);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.Description).HasMaxLength(int.MaxValue);
            entity.HasOne(e => e.Event)
                .WithMany(e => e.LeaderBoards)
                .HasForeignKey(e => e.EventId);
        });
    }
}
