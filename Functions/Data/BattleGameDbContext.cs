using Microsoft.EntityFrameworkCore;
using Functions.Models;

namespace Functions.Data
{
    public class BattleGameDbContext : DbContext
    {
        public BattleGameDbContext(DbContextOptions<BattleGameDbContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<PlayerAsset> PlayerAssets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(eb =>
            {
                eb.ToTable("player");
                eb.HasKey(x => x.PlayerId);
                eb.Property(x => x.PlayerId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Asset>(eb =>
            {
                eb.ToTable("asset");
                eb.HasKey(x => x.AssetId);
                eb.Property(x => x.AssetId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PlayerAsset>(eb =>
            {
                eb.ToTable("playerasset");
                eb.HasKey(x => new { x.PlayerId, x.AssetId });
                eb.HasOne(x => x.Player)
                    .WithMany()
                    .HasForeignKey(x => x.PlayerId)
                    .OnDelete(DeleteBehavior.Cascade);
                eb.HasOne(x => x.Asset)
                    .WithMany()
                    .HasForeignKey(x => x.AssetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
