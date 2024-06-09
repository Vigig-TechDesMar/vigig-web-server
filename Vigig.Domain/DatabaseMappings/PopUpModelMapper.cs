using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.Domain.DatabaseMappings;

public class PopUpModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PopUp>(entity =>
        {
            entity.ToTable(nameof(PopUp));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Title)
                .IsRequired();
            entity.Property(e => e.SubTitle);
            entity.Property(e => e.StartDate)
                .IsRequired();
            entity.Property(e => e.EndDate)
                .IsRequired();
            entity.Property(e => e.EventId);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.HasOne(e => e.Event)
                .WithMany(e => e.PopUps)
                .HasForeignKey(e => e.EventId);
        });
    }
}
