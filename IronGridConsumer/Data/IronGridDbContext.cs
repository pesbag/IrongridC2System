using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronGridConsumer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions;
namespace IronGridConsumer.Data;

public class IronGridDbContext:DbContext
{
    public IronGridDbContext(DbContextOptions<IronGridDbContext> options) : base(options) { }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<AssetLiveStatus> AssetLiveStatuses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Asset>()
            .HasOne(a => a.Unit)
            .WithMany(a => a.assets)
            .HasForeignKey(a => a.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
       
        modelBuilder.Entity<Asset>()
            .HasOne(a => a.assetLive)
            .WithOne(a => a.Asset)
            .HasForeignKey<AssetLiveStatus>();

       
    }
}
