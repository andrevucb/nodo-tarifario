using Microsoft.EntityFrameworkCore;
using Nodo.SharedKernel;
using Nodo.Tarifario.Domain;
using TarifarioAggregate = Nodo.Tarifario.Domain.Tarifario;

namespace Nodo.Tarifario.Infrastructure;

/// <summary>Datos de demostración. Se crean con el propio dominio para respetar sus invariantes.</summary>
public static class TarifarioSeed
{
    public static readonly Guid TarifarioNodoId = Guid.Parse("7a000000-0000-0000-0000-000000000001");

    public static async Task SembrarAsync(TarifarioDbContext ctx, CancellationToken ct = default)
    {
        if (await ctx.Tarifarios.AnyAsync(ct)) return;

        // EF Core rastrea los owned types por referencia: cada tarifa necesita su propia instancia de Franja.
        Franja HorarioNormal() => new(new TimeOnly(8, 0), new TimeOnly(18, 0));
        Franja HoraPico() => new(new TimeOnly(18, 0), new TimeOnly(22, 0));
        Franja JornadaCompleta() => new(new TimeOnly(8, 0), new TimeOnly(22, 0));
        var temporadaAlta = new Temporada(new DateOnly(2026, 12, 1), new DateOnly(2027, 1, 31));

        var tarifario = TarifarioAggregate.Crear(TarifarioNodoId);
        tarifario.AgregarTarifa(new Tarifa(Guid.Empty, "SalaReuniones", false, Bob(50), HorarioNormal()));
        tarifario.AgregarTarifa(new Tarifa(Guid.Empty, "SalaReuniones", true, Bob(80), HoraPico()));
        tarifario.AgregarTarifa(new Tarifa(Guid.Empty, "SalonEventos", false, Bob(1500), JornadaCompleta()));
        tarifario.AgregarTarifa(new Tarifa(Guid.Empty, "SalonEventos", false, Bob(1800), JornadaCompleta(), temporadaAlta));
        tarifario.AgregarTarifa(new Tarifa(Guid.Empty, "EscritorioFlexible", false, Bob(25), HorarioNormal()));
        tarifario.AgregarTarifa(new Tarifa(Guid.Empty, "EscritorioFlexible", true, Bob(35), HoraPico()));

        ctx.Tarifarios.Add(tarifario);
        await ctx.SaveChangesAsync(ct);
    }

    private static Money Bob(decimal monto) => new(monto, "BOB");
}
