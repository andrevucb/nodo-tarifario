using Microsoft.EntityFrameworkCore;
using Nodo.Cotizaciones.Domain;

namespace Nodo.Cotizaciones.Infrastructure;

public class CotizacionesDbContext(DbContextOptions<CotizacionesDbContext> options) : DbContext(options)
{
    public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("cotizaciones");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CotizacionesDbContext).Assembly);
    }
}
