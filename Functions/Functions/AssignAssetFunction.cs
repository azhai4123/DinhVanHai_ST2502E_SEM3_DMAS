using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Functions.Data;
using Functions.Helpers;
using Functions.Models;

namespace Functions.Functions
{
    public class AssignAssetFunction
    {
        private readonly BattleGameDbContext _db;

        public AssignAssetFunction(BattleGameDbContext db)
        {
            _db = db;
        }

        public class AssignRequest
        {
            public Guid PlayerId { get; set; }
            public Guid AssetId { get; set; }
            public int Quantity { get; set; } = 1;
        }

        [Function("AssignAsset")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", "options", Route = "assignasset")] HttpRequestData req)
        {
            if (req.Method == "OPTIONS")
            {
                var optionsResponse = req.CreateResponse(HttpStatusCode.OK);
                optionsResponse.AddCors();
                return optionsResponse;
            }

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var r = JsonSerializer.Deserialize<AssignRequest>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (r == null || r.PlayerId == Guid.Empty || r.AssetId == Guid.Empty)
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                bad.AddCors();
                await bad.WriteStringAsync("Invalid payload");
                return bad;
            }

            var player = await _db.Players.FindAsync(r.PlayerId);
            var asset = await _db.Assets.FindAsync(r.AssetId);
            if (player == null || asset == null)
            {
                var notf = req.CreateResponse(HttpStatusCode.NotFound);
                notf.AddCors();
                await notf.WriteStringAsync("Player or Asset not found");
                return notf;
            }

            var pa = new PlayerAsset { PlayerId = r.PlayerId, AssetId = r.AssetId, Quantity = r.Quantity, AcquiredDate = DateTime.UtcNow };
            _db.PlayerAssets.Add(pa);
            await _db.SaveChangesAsync();

            var res = req.CreateResponse(HttpStatusCode.Created);
            res.Headers.Add("Content-Type", "application/json");
            res.AddCors();
            await res.WriteStringAsync(JsonSerializer.Serialize(new { playerId = pa.PlayerId, assetId = pa.AssetId }));
            return res;
        }
    }
}
