using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class CustomerTokenModalMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerToken>(entity =>
        {
            entity.ToTable("CustomerToken");
            entity.HasKey(e => new { e.CustomerId, e.Name, e.LoginProvider });
            entity.Property(e => e.CustomerId).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.LoginProvider).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Value).IsRequired().HasMaxLength(Int32.MaxValue);

            entity.HasOne<Customer>().WithMany().HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}