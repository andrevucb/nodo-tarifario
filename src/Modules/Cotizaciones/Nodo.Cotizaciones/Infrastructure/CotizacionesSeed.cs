using Microsoft.EntityFrameworkCore;
using Nodo.Cotizaciones.Domain;
using Nodo.SharedKernel;

namespace Nodo.Cotizaciones.Infrastructure;

/// <summary>Una cotización por cada camino del ciclo de vida, creadas con el propio aggregate.</summary>
public static class CotizacionesSeed
{
    // Mismos identificadores que los adaptadores InMemory de Cálculo de precio total.
    private static readonly Guid SalaReuniones = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid SalonEventos = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid ClienteEquipo = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid ClienteIndividual = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static async Task SembrarAsync(CotizacionesDbContext ctx, CancellationToken ct = default)
    {
        if (await ctx.Cotizaciones.AnyAsync(ct)) return;

        var ahora = DateTime.UtcNow;

        var vigente = Cotizacion.Emitir(Guid.Empty, ClienteEquipo, SalaReuniones, Bob(90), ahora.AddDays(-1));

        var aceptada = Cotizacion.Emitir(Guid.Empty, ClienteIndividual, SalonEventos, Bob(1500), ahora.AddDays(-2));
        aceptada.Aceptar(ahora.AddDays(-1));

        // Vencida y ya registrada como caducada.
        var caducada = Cotizacion.Emitir(Guid.Empty, ClienteEquipo, SalonEventos, Bob(1350), ahora.AddDays(-10));
        caducada.RegistrarCaducidad(ahora);

        // Vencida pero aún sin registrar: sirve para demostrar INV-05 al intentar aceptarla.
        var vencidaSinRegistrar = Cotizacion.Emitir(Guid.Empty, ClienteIndividual, SalaReuniones, Bob(190), ahora.AddDays(-6));

        foreach (var c in new[] { vigente, aceptada, caducada, vencidaSinRegistrar })
        {
            c.ClearDomainEvents();
            ctx.Cotizaciones.Add(c);
        }
        await ctx.SaveChangesAsync(ct);
    }

    private static Money Bob(decimal monto) => new(monto, "BOB");
}
