using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Paschoalotto.People.CrossCutting;
using Paschoalotto.People.Infrastructure.Persistence;
using Paschoalotto.People.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Paschoalotto People API", Version = "v1" });
});

builder.Services.AddPeoplePlatform(builder.Configuration);

var storageRoot = builder.Configuration["Storage:Root"] ?? "./_storage";
var physicalPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, storageRoot));
Directory.CreateDirectory(physicalPath);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(physicalPath),
    RequestPath = "/files",
    ServeUnknownFileTypes = false
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PeopleDbContext>();
    await db.Database.MigrateAsync();
    await SeedData.ApplyAsync(db);
}
app.Run();
