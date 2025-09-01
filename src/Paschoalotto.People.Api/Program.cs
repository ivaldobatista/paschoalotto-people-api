using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
