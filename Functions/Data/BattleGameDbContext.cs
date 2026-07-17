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
                eb.HasKey(x => x.PlayerId);
                eb.Property(x => x.PlayerId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Asset>(eb =>
            {
                eb.HasKey(x => x.AssetId);
                eb.Property(x => x.AssetId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PlayerAsset>(eb =>
            {
                eb.HasKey(x => x.PlayerAssetId);
                eb.HasOne(x => x.Player).WithMany().HasForeignKey(x => x.PlayerId);
                eb.HasOne(x => x.Asset).WithMany().HasForeignKey(x => x.AssetId);
            });
        }
    }
}
