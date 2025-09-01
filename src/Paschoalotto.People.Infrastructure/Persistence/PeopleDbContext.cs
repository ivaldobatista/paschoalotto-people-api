using Microsoft.EntityFrameworkCore;
using Paschoalotto.People.Domain.People;

namespace Paschoalotto.People.Infrastructure.Persistence;

public sealed class PeopleDbContext : DbContext
{
    public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options) { }

    public DbSet<Individual> Individuals => Set<Individual>();
    public DbSet<LegalEntity> LegalEntities => Set<LegalEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PeopleDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State == EntityState.Modified))
        {
            var prop = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedAtUtc");
            if (prop is not null) prop.CurrentValue = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
