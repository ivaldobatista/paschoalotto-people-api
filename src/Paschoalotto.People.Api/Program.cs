using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Paschoalotto.People.CrossCutting;
using Paschoalotto.People.Infrastructure.Persistence;
using Paschoalotto.People.Infrastructure.Seed;
using System.Text;
using NLog;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Paschoalotto People API", Version = "v1" });
        var scheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Informe: Bearer {seu_token}"
        };
        c.AddSecurityDefinition("Bearer", scheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            [scheme] = new List<string>()
        });
    });

    builder.Services.AddPeoplePlatform(builder.Configuration);

    var storageRoot = builder.Configuration["Storage:Root"] ?? "./_storage";
    var physicalPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, storageRoot));
    Directory.CreateDirectory(physicalPath);

    var cfg = builder.Configuration;

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = cfg["Jwt:Issuer"],
                ValidAudience = cfg["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!)),
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("People.Read", p => p.RequireAuthenticatedUser().RequireRole("admin", "operator"));
        options.AddPolicy("People.Write", p => p.RequireAuthenticatedUser().RequireRole("admin"));
    });

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

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<PeopleDbContext>();
        await db.Database.MigrateAsync();
        await SeedData.ApplyAsync(db);
    }
    app.Run();
}
catch (Exception ex)
{
    // last-chance logging (startup crash)
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

public partial class Program { }