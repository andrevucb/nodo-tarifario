using Microsoft.EntityFrameworkCore;
using Nodo.Tarifario.Domain;
using TarifarioAggregate = Nodo.Tarifario.Domain.Tarifario;

namespace Nodo.Tarifario.Infrastructure;

public class TarifarioDbContext(DbContextOptions<TarifarioDbContext> options) : DbContext(options)
{
    public DbSet<TarifarioAggregate> Tarifarios => Set<TarifarioAggregate>();
    public DbSet<Tarifa> Tarifas => Set<Tarifa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("tarifario");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TarifarioDbContext).Assembly);
    }
}
