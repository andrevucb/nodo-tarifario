using Microsoft.EntityFrameworkCore;
using Nodo.Tarifario.Domain;
using TarifarioAggregate = Nodo.Tarifario.Domain.Tarifario;

namespace Nodo.Tarifario.Infrastructure.Repositories;

public class EfRepositorioTarifario(TarifarioDbContext ctx) : IRepositorioTarifario
{
    public Task<TarifarioAggregate?> ObtenerPorIdAsync(Guid tarifarioId, CancellationToken ct = default) =>
        ctx.Tarifarios.Include(t => t.Tarifas).FirstOrDefaultAsync(t => t.Id == tarifarioId, ct);

    public async Task GuardarAsync(TarifarioAggregate tarifario, CancellationToken ct = default)
    {
        if (!ctx.Tarifarios.Local.Contains(tarifario))
        {
            var exists = await ctx.Tarifarios.AnyAsync(t => t.Id == tarifario.Id, ct);
            if (!exists) ctx.Tarifarios.Add(tarifario);
            else ctx.Tarifarios.Update(tarifario);
        }
        await ctx.SaveChangesAsync(ct);
    }
}
