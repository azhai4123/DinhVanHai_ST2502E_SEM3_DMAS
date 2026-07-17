using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Functions.Worker.Configuration;
using Functions.Data;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        builder.AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        var conn = config.GetConnectionString("BattleGame") ?? Environment.GetEnvironmentVariable("ConnectionStrings__BattleGame");
        if (string.IsNullOrEmpty(conn))
        {
            conn = "server=127.0.0.1;port=3306;user=root;password=;database=BATTLEGAME;";
        }

        services.AddDbContext<BattleGameDbContext>(options => options.UseMySql(conn, ServerVersion.AutoDetect(conn)));
    })
    .Build();

// Ensure database created and seed sample data (for local dev)
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<Functions.Data.BattleGameDbContext>();
        db.Database.EnsureCreated();
        await Functions.Data.DbInitializer.SeedAsync(db);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding DB: {ex}");
    }
}

host.Run();
