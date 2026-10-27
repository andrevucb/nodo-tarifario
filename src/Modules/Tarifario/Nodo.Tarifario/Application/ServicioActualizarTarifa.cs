using Nodo.SharedKernel;
using Nodo.Tarifario.Domain;

namespace Nodo.Tarifario.Application;

public class ServicioActualizarTarifa
{
    private readonly IRepositorioTarifario _repo;
    private readonly IDomainEventDispatcher _dispatcher;

    public ServicioActualizarTarifa(IRepositorioTarifario repo, IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _dispatcher = dispatcher;
    }

    public async Task ActualizarAsync(Guid tarifarioId, Guid tarifaId, decimal nuevoImporte, string moneda, string solicitanteUsuarioId, string solicitanteRol, CancellationToken ct = default)
    {
        var tarifario = await _repo.ObtenerPorIdAsync(tarifarioId, ct) ?? throw new DomainException("Tarifario no encontrado.");
        var money = new Money(nuevoImporte, moneda);
        tarifario.ActualizarTarifa(tarifaId, money, solicitanteUsuarioId, solicitanteRol);
        await _repo.GuardarAsync(tarifario, ct);

        foreach (var ev in tarifario.DomainEvents)
            await _dispatcher.DispatchAsync(ev, ct);
        tarifario.ClearDomainEvents();
    }
}
