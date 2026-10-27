using Nodo.CalculoPrecioTotal.Domain.Contracts;

namespace Nodo.CalculoPrecioTotal.Infrastructure.Adapters;

public class InMemoryEspaciosQuery : IEspaciosQuery
{
    private readonly Dictionary<Guid, RecursoInfo> _recursos = [];

    public InMemoryEspaciosQuery()
    {
        _recursos[Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")] = new RecursoInfo(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "SalaReuniones", 6);
        _recursos[Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")] = new RecursoInfo(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "SalonEventos", 50);
    }

    public Task<RecursoInfo?> ObtenerRecursoAsync(Guid recursoId, CancellationToken ct = default) =>
        Task.FromResult(_recursos.TryGetValue(recursoId, out var r) ? r : null);
}
