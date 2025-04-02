using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace greenhouse_aspnet_api.db.Models;

public class GreenhouseDbContext : DbContext
{
  public DbSet<Device> Devices { get; set; }

  public GreenhouseDbContext(DbContextOptions<GreenhouseDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Device>()
        .Property(d => d.Type)
        .HasConversion(
            v => v.ToString().ToLower(),
            v => (DeviceType)Enum.Parse(typeof(DeviceType), v, true)
        )
        .HasColumnName("type");

    modelBuilder.Entity<Device>()
        .Property(d => d.Status)
        .HasConversion(
            v => v.ToString().ToLower(),
            v => (DeviceStatus)Enum.Parse(typeof(DeviceStatus), v, true)
        )
        .HasColumnName("status");

    modelBuilder.Entity<Device>()
        .Property(d => d.SearchVector)
        .HasComputedColumnSql("to_tsvector('simple', coalesce(name, '') || ' ' || coalesce(code, ''))", stored: true);

    modelBuilder.Entity<Device>()
        .HasIndex(d => d.SearchVector)
        .HasMethod("GIN");
  }
}
