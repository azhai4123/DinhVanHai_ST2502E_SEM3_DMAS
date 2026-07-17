using Functions.Models;
using Microsoft.EntityFrameworkCore;

namespace Functions.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(BattleGameDbContext db)
        {
            if (await db.Players.AnyAsync()) return;

            var p1 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "Player 1", FullName = "Nguyen Van A", Age = 20, Level = 10, Email = "p1@example.com" };
            var p2 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "Player 2", FullName = "Tran Thi B", Age = 19, Level = 3, Email = "p2@example.com" };
            var p3 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "Player 3", FullName = "Le Van C", Age = 23, Level = 10, Email = "p3@example.com" };

            var a1 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Hero 1", LevelRequire = 1 };
            var a2 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Hero 2", LevelRequire = 2 };

            db.Players.AddRange(p1, p2, p3);
            db.Assets.AddRange(a1, a2);

            var pa1 = new PlayerAsset { PlayerAssetId = Guid.NewGuid(), PlayerId = p1.PlayerId, AssetId = a1.AssetId, AcquiredDate = DateTime.UtcNow, Quantity = 1 };
            var pa2 = new PlayerAsset { PlayerAssetId = Guid.NewGuid(), PlayerId = p2.PlayerId, AssetId = a2.AssetId, AcquiredDate = DateTime.UtcNow, Quantity = 1 };
            var pa3 = new PlayerAsset { PlayerAssetId = Guid.NewGuid(), PlayerId = p3.PlayerId, AssetId = a1.AssetId, AcquiredDate = DateTime.UtcNow, Quantity = 1 };

            db.PlayerAssets.AddRange(pa1, pa2, pa3);

            await db.SaveChangesAsync();
        }
    }
}
