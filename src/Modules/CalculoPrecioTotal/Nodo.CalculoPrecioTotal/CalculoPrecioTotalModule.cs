using Microsoft.Extensions.DependencyInjection;
using Nodo.CalculoPrecioTotal.Domain;
using Nodo.CalculoPrecioTotal.Domain.Contracts;
using Nodo.CalculoPrecioTotal.Infrastructure.Adapters;

namespace Nodo.CalculoPrecioTotal;

public static class CalculoPrecioTotalModule
{
    public static IServiceCollection AddCalculoPrecioTotalModule(this IServiceCollection services)
    {
        services.AddSingleton<ITarifasQuery, InMemoryTarifasQuery>();
        services.AddSingleton<IMembresiasQuery, InMemoryMembresiasQuery>();
        services.AddSingleton<IEspaciosQuery, InMemoryEspaciosQuery>();
        services.AddScoped<ServicioCalculoPrecio>();
        return services;
    }
}
