namespace Nodo.Cotizaciones.Domain;

public interface IRepositorioCotizacion
{
    Task<Cotizacion?> ObtenerPorIdAsync(Guid cotizacionId, CancellationToken ct = default);
    Task GuardarAsync(Cotizacion cotizacion, CancellationToken ct = default);
}
