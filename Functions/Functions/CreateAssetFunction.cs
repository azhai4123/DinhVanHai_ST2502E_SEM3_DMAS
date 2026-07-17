using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Functions.Data;
using Functions.Helpers;
using Functions.Models;

namespace Functions.Functions
{
    public class CreateAssetFunction
    {
        private readonly BattleGameDbContext _db;

        public CreateAssetFunction(BattleGameDbContext db)
        {
            _db = db;
        }

        [Function("CreateAsset")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", "options", Route = "createasset")] HttpRequestData req)
        {
            if (req.Method == "OPTIONS")
            {
                var optionsResponse = req.CreateResponse(HttpStatusCode.OK);
                optionsResponse.AddCors();
                return optionsResponse;
            }

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var asset = JsonSerializer.Deserialize<Asset>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (asset == null || string.IsNullOrWhiteSpace(asset.AssetName))
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                bad.AddCors();
                await bad.WriteStringAsync("Invalid asset payload");
                return bad;
            }

            asset.AssetId = asset.AssetId == Guid.Empty ? Guid.NewGuid() : asset.AssetId;
            _db.Assets.Add(asset);
            await _db.SaveChangesAsync();

            var res = req.CreateResponse(HttpStatusCode.Created);
            res.Headers.Add("Content-Type", "application/json");
            res.AddCors();
            await res.WriteStringAsync(JsonSerializer.Serialize(new { assetId = asset.AssetId }));
            return res;
        }
    }
}
