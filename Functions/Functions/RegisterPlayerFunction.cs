using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Functions.Data;
using Functions.Helpers;
using Functions.Models;

namespace Functions.Functions
{
    public class RegisterPlayerFunction
    {
        private readonly BattleGameDbContext _db;

        public RegisterPlayerFunction(BattleGameDbContext db)
        {
            _db = db;
        }

        [Function("RegisterPlayer")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", "options", Route = "registerplayer")] HttpRequestData req)
        {
            if (req.Method == "OPTIONS")
            {
                var optionsResponse = req.CreateResponse(HttpStatusCode.OK);
                optionsResponse.AddCors();
                return optionsResponse;
            }

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var player = JsonSerializer.Deserialize<Player>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (player == null || string.IsNullOrWhiteSpace(player.PlayerName))
            {
                var bad = req.CreateResponse(HttpStatusCode.BadRequest);
                bad.AddCors();
                await bad.WriteStringAsync("Invalid player payload");
                return bad;
            }

            player.PlayerId = player.PlayerId == Guid.Empty ? Guid.NewGuid() : player.PlayerId;
            _db.Players.Add(player);
            await _db.SaveChangesAsync();

            var res = req.CreateResponse(HttpStatusCode.Created);
            res.Headers.Add("Content-Type", "application/json");
            res.AddCors();
            await res.WriteStringAsync(JsonSerializer.Serialize(new { playerId = player.PlayerId }));
            return res;
        }
    }
}
