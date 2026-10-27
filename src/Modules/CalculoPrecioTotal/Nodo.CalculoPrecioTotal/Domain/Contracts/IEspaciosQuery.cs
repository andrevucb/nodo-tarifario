namespace Nodo.CalculoPrecioTotal.Domain.Contracts;

public record RecursoInfo(Guid RecursoId, string TipoRecurso, int Capacidad);

public interface IEspaciosQuery
{
    Task<RecursoInfo?> ObtenerRecursoAsync(Guid recursoId, CancellationToken ct = default);
}
