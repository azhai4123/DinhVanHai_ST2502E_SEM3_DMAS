using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Functions.Data;
using Functions.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Functions.Functions
{
    public class ListPlayersFunction
    {
        private readonly BattleGameDbContext _db;
        public ListPlayersFunction(BattleGameDbContext db) => _db = db;

        [Function("ListPlayers")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "options", Route = "players")] HttpRequestData req)
        {
            if (req.Method == "OPTIONS")
            {
                var optionsResponse = req.CreateResponse(HttpStatusCode.OK);
                optionsResponse.AddCors();
                return optionsResponse;
            }

            var list = await _db.Players.ToListAsync();
            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "application/json");
            res.AddCors();
            await res.WriteStringAsync(JsonSerializer.Serialize(list));
            return res;
        }
    }
}
