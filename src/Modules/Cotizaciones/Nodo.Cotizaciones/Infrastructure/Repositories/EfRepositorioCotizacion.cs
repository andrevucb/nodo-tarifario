using Microsoft.EntityFrameworkCore;
using Nodo.Cotizaciones.Domain;

namespace Nodo.Cotizaciones.Infrastructure.Repositories;

public class EfRepositorioCotizacion(CotizacionesDbContext ctx) : IRepositorioCotizacion
{
    public Task<Cotizacion?> ObtenerPorIdAsync(Guid cotizacionId, CancellationToken ct = default) =>
        ctx.Cotizaciones.FirstOrDefaultAsync(c => c.Id == cotizacionId, ct);

    public async Task GuardarAsync(Cotizacion cotizacion, CancellationToken ct = default)
    {
        if (!ctx.Cotizaciones.Local.Contains(cotizacion))
        {
            var exists = await ctx.Cotizaciones.AnyAsync(c => c.Id == cotizacion.Id, ct);
            if (!exists) ctx.Cotizaciones.Add(cotizacion);
            else ctx.Cotizaciones.Update(cotizacion);
        }
        await ctx.SaveChangesAsync(ct);
    }
}
