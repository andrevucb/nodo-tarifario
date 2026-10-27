namespace Nodo.Tarifario.Domain;

public interface IRepositorioTarifario
{
    Task<Tarifario?> ObtenerPorIdAsync(Guid tarifarioId, CancellationToken ct = default);
    Task GuardarAsync(Tarifario tarifario, CancellationToken ct = default);
}
