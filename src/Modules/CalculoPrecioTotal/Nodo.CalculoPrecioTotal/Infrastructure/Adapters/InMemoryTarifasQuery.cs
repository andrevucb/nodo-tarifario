using Nodo.SharedKernel;
using Nodo.CalculoPrecioTotal.Domain.Contracts;

namespace Nodo.CalculoPrecioTotal.Infrastructure.Adapters;

public class InMemoryTarifasQuery : ITarifasQuery
{
    private readonly List<TarifaAplicable> _tarifas = [];

    public InMemoryTarifasQuery()
    {
        // Datos de prueba
        _tarifas.Add(new TarifaAplicable(Guid.NewGuid(), Guid.NewGuid(), "SalaReuniones", false, new TimeOnly(8, 0),
            new TimeOnly(18, 0), null, null, new Money(50, "BOB")));
        _tarifas.Add(new TarifaAplicable(Guid.NewGuid(), Guid.NewGuid(), "SalaReuniones", true, new TimeOnly(18, 0),
            new TimeOnly(22, 0), null, null, new Money(80, "BOB")));
        _tarifas.Add(new TarifaAplicable(Guid.NewGuid(), Guid.NewGuid(), "SalonEventos", false, new TimeOnly(8, 0),
            new TimeOnly(22, 0), null, null, new Money(1500, "BOB")));
        _tarifas.Add(new TarifaAplicable(Guid.NewGuid(), Guid.NewGuid(), "EscritorioFlexible", false,
            new TimeOnly(8, 0), new TimeOnly(18, 0), null, null, new Money(25, "BOB")));
        _tarifas.Add(new TarifaAplicable(Guid.NewGuid(), Guid.NewGuid(), "EscritorioFlexible", true,
            new TimeOnly(18, 0), new TimeOnly(22, 0), null, null, new Money(35, "BOB")));
    }

    public Task<TarifaAplicable?> ObtenerTarifaAplicableAsync(string tipoRecurso, DateTime fechaHora,
        CancellationToken ct = default)
    {
        var hora = TimeOnly.FromDateTime(fechaHora);
        var tarifa = _tarifas
            .Where(t => string.Equals(t.TipoRecurso, tipoRecurso, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault(t => hora >= t.FranjaInicio && hora < t.FranjaFin);
        return Task.FromResult(tarifa);
    }
}