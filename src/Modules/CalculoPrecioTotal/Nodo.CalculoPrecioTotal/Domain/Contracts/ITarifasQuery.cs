using Nodo.SharedKernel;

namespace Nodo.CalculoPrecioTotal.Domain.Contracts;

public record TarifaAplicable(Guid TarifaId, Guid TarifarioId, string TipoRecurso, bool EsHoraPico, TimeOnly FranjaInicio, TimeOnly FranjaFin, DateOnly? TemporadaInicio, DateOnly? TemporadaFin, Money TarifaBase);

public interface ITarifasQuery
{
    Task<TarifaAplicable?> ObtenerTarifaAplicableAsync(string tipoRecurso, DateTime fechaHora, CancellationToken ct = default);
}
