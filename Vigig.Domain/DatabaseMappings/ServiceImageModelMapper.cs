using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class ServiceImageModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceImage>(entity =>
        {
            entity.ToTable("ServiceImage");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ProviderService).WithMany(p => p.ServiceImages)
                .HasForeignKey(d => d.ProviderServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceIm__Provi__68487DD7");
        });
    }
}