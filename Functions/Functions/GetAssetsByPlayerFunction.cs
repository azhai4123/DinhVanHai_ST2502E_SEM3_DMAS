using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Functions.Data;
using Microsoft.EntityFrameworkCore;

namespace Functions.Functions
{
    public class GetAssetsByPlayerFunction
    {
        private readonly BattleGameDbContext _db;

        public GetAssetsByPlayerFunction(BattleGameDbContext db)
        {
            _db = db;
        }

        [Function("GetAssetsByPlayer")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getassetsbyplayer")] HttpRequestData req)
        {
            var query = from p in _db.Players
                        join pa in _db.PlayerAssets on p.PlayerId equals pa.PlayerId
                        join a in _db.Assets on pa.AssetId equals a.AssetId
                        select new { p.PlayerName, p.Level, p.Age, a.AssetName };

            var list = await query.OrderBy(x => x.PlayerName).ToListAsync();

            var report = list.Select((x, idx) => new { No = idx + 1, PlayerName = x.PlayerName, Level = x.Level, Age = x.Age, AssetName = x.AssetName });

            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json");
            await res.WriteStringAsync(JsonSerializer.Serialize(report));
            return res;
        }
    }
}
