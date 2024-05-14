﻿using Microsoft.EntityFrameworkCore;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.Domain.DatabaseMappings;

public class NotificationTypeModelMapper : IDatabaseModelMapper
{
    public void Map(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.ToTable("NotificationType");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });
    }
}