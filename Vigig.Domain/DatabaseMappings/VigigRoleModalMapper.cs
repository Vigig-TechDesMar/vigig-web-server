using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class VigigRoleModalMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VigigRole>(entity =>
        {
            entity.ToTable("VigigRole");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.NormalizedName).HasMaxLength(255);

            entity.HasMany<VigigUser>(r => r.VigigUsers)
                .WithMany(u => u.Roles)
                .UsingEntity("UserRole",
                    l => l.HasOne(typeof(VigigUser)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(VigigUser.Id)),
                    r => r.HasOne(typeof(VigigRole)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(VigigRole.Id)),
                    j => j.HasKey("UserId","RoleId")
                    );
        });
    }
}