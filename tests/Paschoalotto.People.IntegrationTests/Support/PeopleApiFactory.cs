using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paschoalotto.People.Infrastructure.Persistence;

public class PeopleApiFactory : WebApplicationFactory<Program>
{
    private string _tempRoot = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _tempRoot = Path.Combine(Path.GetTempPath(), "people_api_tests", Guid.NewGuid().ToString("N"));

        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            Directory.CreateDirectory(_tempRoot);

            var inMemory = new Dictionary<string, string?>
            {
                ["ConnectionStrings:PeopleDb"] = $"Data Source={Path.Combine(_tempRoot, "people_tests.db")}",
                ["Storage:Root"] = _tempRoot
            };

            cfg.AddInMemoryCollection(inMemory!);
        });

        builder.ConfigureServices(services =>
        {
            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PeopleDbContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && !string.IsNullOrEmpty(_tempRoot) && Directory.Exists(_tempRoot))
        {
            try { Directory.Delete(_tempRoot, recursive: true); } catch { /* ignore em teardown */ }
        }
    }
}
