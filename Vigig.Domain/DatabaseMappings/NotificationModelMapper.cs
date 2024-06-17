using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Entities;
using Vigig.Domain.Interfaces;

namespace Vigig.Domain.DatabaseMappings;

public class NotificationModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Content);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.RedirectUrl);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne<NotificationType>(e => e.NotificationType)
                .WithMany(e => e.Notifications)
                .HasForeignKey(e => e.NotificationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne<VigigUser>(e => e.VigigUser)
                .WithMany(e => e.Notifications)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}