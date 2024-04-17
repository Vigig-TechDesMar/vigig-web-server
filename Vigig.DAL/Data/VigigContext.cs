using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NLog;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Interfaces;
using Vigig.Domain.Models;

namespace Vigig.DAL.Data;

public partial class VigigContext : DbContext,IAppDbContext
{
    private readonly ILogger logger = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);
    public VigigContext()
    {
    }

    public VigigContext(DbContextOptions<VigigContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (optionsBuilder.IsConfigured)
            return;
        optionsBuilder.UseSqlServer(DataAccessHelper.GetDefaultConnectionString())
            .EnableSensitiveDataLogging()
            .LogTo(Console.Write);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var modelMappers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(IDatabaseModelMapper)));
        foreach (var modelMapper in modelMappers)
        {
            var instance = Activator.CreateInstance(modelMapper) as IDatabaseModelMapper;
            instance?.Map(modelBuilder);
        }
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public DbSet<T> CreateSet<T>() where T : class
    {
        return base.Set<T>();
    }

    public new void Attach<T>(T entity) where T : class
    {
        base.Attach(entity);
    }

    public void SetModified<T>(T entity) where T : class
    {
        base.Entry(entity).State = EntityState.Modified;
    }

    public void SetDeleted<T>(T entity) where T : class
    {
        base.Entry(entity).State = EntityState.Deleted;
    }

    public void Refresh<T>(T entity) where T : class
    {
        base.Entry(entity).Reload();
    }

    public new void Update<T>(T entity) where T : class
    {
        base.Update(entity);
    }

    public new void SaveChanges()
    {
        base.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}
