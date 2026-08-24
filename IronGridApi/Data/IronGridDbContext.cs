using IronGridApi.Models;
using Microsoft.EntityFrameworkCore;
namespace IronGridApi.Data;

public class IronGridDbContext : DbContext
{
    public IronGridDbContext(DbContextOptions<IronGridDbContext> options) : base(options) { }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<AssetLiveStatus> AssetLiveStatus { get; set; }
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
        modelBuilder.Entity<AssetLiveStatus>(entity =>
        {
            entity.HasKey(c => c.AssetId);
            entity.Property(c => c.AssetId).ValueGeneratedNever();
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
