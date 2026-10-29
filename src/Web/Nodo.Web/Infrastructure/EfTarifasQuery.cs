using Microsoft.EntityFrameworkCore;
using Nodo.CalculoPrecioTotal.Domain.Contracts;
using Nodo.Tarifario.Infrastructure;

namespace Nodo.Web.Infrastructure;

public sealed class EfTarifasQuery(TarifarioDbContext context) : ITarifasQuery
{
    public async Task<TarifaAplicable?> ObtenerTarifaAplicableAsync(
        string tipoRecurso,
        DateTime fechaHora,
        CancellationToken ct = default)
    {
        var hora = TimeOnly.FromDateTime(fechaHora);
        var fecha = DateOnly.FromDateTime(fechaHora);
        var tarifarios = await context.Tarifarios
            .AsNoTracking()
            .Include(tarifario => tarifario.Tarifas)
            .ToListAsync(ct);

        var seleccion = tarifarios
            .SelectMany(tarifario => tarifario.Tarifas.Select(tarifa => (tarifario, tarifa)))
            .Where(x => string.Equals(x.tarifa.TipoRecurso, tipoRecurso, StringComparison.OrdinalIgnoreCase))
            .Where(x => x.tarifa.Franja.Cubre(hora))
            .Where(x => x.tarifa.Temporada is null || x.tarifa.Temporada.Cubre(fecha))
            .OrderByDescending(x => x.tarifa.Temporada is not null)
            .Select(x => new TarifaAplicable(
                x.tarifa.TarifaId,
                x.tarifario.Id,
                x.tarifa.TipoRecurso,
                x.tarifa.EsHoraPico,
                x.tarifa.Franja.Inicio,
                x.tarifa.Franja.Fin,
                x.tarifa.Temporada?.Inicio,
                x.tarifa.Temporada?.Fin,
                x.tarifa.TarifaBase))
            .FirstOrDefault();

        return seleccion;
    }
}