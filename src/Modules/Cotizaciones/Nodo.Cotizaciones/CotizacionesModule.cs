using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nodo.Cotizaciones.Application;
using Nodo.Cotizaciones.Domain;
using Nodo.Cotizaciones.Infrastructure;
using Nodo.Cotizaciones.Infrastructure.Repositories;

namespace Nodo.Cotizaciones;

public static class CotizacionesModule
{
    public static IServiceCollection AddCotizacionesModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CotizacionesDbContext>(o =>
            o.UseSqlite(connectionString, sqlite =>
                sqlite.MigrationsHistoryTable("__EFMigrationsHistory", "cotizaciones")));

        services.AddScoped<IRepositorioCotizacion, EfRepositorioCotizacion>();
        services.AddScoped<ServicioCotizacion>();
        return services;
    }

    public static async Task MigrateCotizacionesModuleAsync(this IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        await scope.ServiceProvider.GetRequiredService<CotizacionesDbContext>().Database.MigrateAsync();
    }
}
