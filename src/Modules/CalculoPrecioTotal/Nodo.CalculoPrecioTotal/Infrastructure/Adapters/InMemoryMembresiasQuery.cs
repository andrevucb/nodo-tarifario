using Nodo.CalculoPrecioTotal.Domain.Contracts;

namespace Nodo.CalculoPrecioTotal.Infrastructure.Adapters;

public class InMemoryMembresiasQuery : IMembresiasQuery
{
    private readonly Dictionary<Guid, PlanInfo> _planes = [];

    public InMemoryMembresiasQuery()
    {
        // Cliente de prueba con plan de equipo
        _planes[Guid.Parse("11111111-1111-1111-1111-111111111111")] = new PlanInfo("Equipo", 4, 10m);
        _planes[Guid.Parse("22222222-2222-2222-2222-222222222222")] = new PlanInfo("Individual", 2, 5m);
    }

    public Task<PlanInfo?> ObtenerPlanDelMiembroAsync(Guid clienteId, CancellationToken ct = default) =>
        Task.FromResult(_planes.TryGetValue(clienteId, out var p) ? p : null);
}
