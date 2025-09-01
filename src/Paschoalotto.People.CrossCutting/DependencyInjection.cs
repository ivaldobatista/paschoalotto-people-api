using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paschoalotto.People.Application.Abstractions;
using Paschoalotto.People.Application.Abstractions.Repositories;
using Paschoalotto.People.Infrastructure;
using Paschoalotto.People.Infrastructure.Logging;
using Paschoalotto.People.Infrastructure.Persistence;
using Paschoalotto.People.Infrastructure.Repositories;
using Paschoalotto.People.Infrastructure.Security;
using Paschoalotto.People.Infrastructure.Storage;

namespace Paschoalotto.People.CrossCutting;

public static class DependencyInjection
{
    public static IServiceCollection AddPeoplePlatform(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<PeopleDbContext>(opt =>
        {
            opt.UseSqlite(cfg.GetConnectionString("PeopleDb"));
        });

        services.AddScoped<IPersonReadRepository, PersonReadRepository>();
        services.AddScoped<IPersonWriteRepository, PersonWriteRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IFileStorageService, FileSystemStorageService>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddSingleton<IAuditLogger, NLogAuditLogger>();

        return services;
    }
}
