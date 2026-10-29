using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nodo.Tarifario.Application;
using Nodo.Tarifario.Contracts;
using Nodo.Tarifario.Domain;
using Nodo.Tarifario.Infrastructure;
using Nodo.Tarifario.Infrastructure.Queries;
using Nodo.Tarifario.Infrastructure.Repositories;

namespace Nodo.Tarifario;

public static class TarifarioModule
{
    public static IServiceCollection AddTarifarioModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TarifarioDbContext>(o =>
            o.UseSqlite(connectionString, sqlite =>
                sqlite.MigrationsHistoryTable("__EFMigrationsHistory", "tarifario")));

        services.AddScoped<IRepositorioTarifario, EfRepositorioTarifario>();
        services.AddScoped<ITarifasQuery, EfTarifasQuery>();
        services.AddScoped<ServicioActualizarTarifa>();
        return services;
    }

    public static async Task MigrateTarifarioModuleAsync(this IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        await scope.ServiceProvider.GetRequiredService<TarifarioDbContext>().Database.MigrateAsync();
    }

    public static async Task SeedTarifarioModuleAsync(this IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        await TarifarioSeed.SembrarAsync(scope.ServiceProvider.GetRequiredService<TarifarioDbContext>());
    }
}
