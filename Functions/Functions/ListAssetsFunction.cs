using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Functions.Data;
using Functions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Functions.Functions
{
    public class ListAssetsFunction
    {
        private readonly BattleGameDbContext _db;
        public ListAssetsFunction(BattleGameDbContext db) => _db = db;

        [Function("ListAssets")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "options", Route = "assets")] HttpRequestData req)
        {
            if (req.Method == "OPTIONS")
            {
                var optionsResponse = req.CreateResponse(HttpStatusCode.OK);
                optionsResponse.AddCors();
                return optionsResponse;
            }

            var list = await _db.Assets.ToListAsync();
            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json");
            res.AddCors();
            await res.WriteStringAsync(JsonSerializer.Serialize(list));
            return res;
        }
    }
}
