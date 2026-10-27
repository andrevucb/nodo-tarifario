using Nodo.SharedKernel;
using Nodo.Cotizaciones.Domain;
using Nodo.CalculoPrecioTotal.Domain;

namespace Nodo.Cotizaciones.Application;

public class ServicioCotizacion
{
    private readonly IRepositorioCotizacion _repo;
    private readonly ServicioCalculoPrecio _calculo;
    private readonly IDomainEventDispatcher _dispatcher;

    public ServicioCotizacion(IRepositorioCotizacion repo, ServicioCalculoPrecio calculo, IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _calculo = calculo;
        _dispatcher = dispatcher;
    }

    public async Task<Cotizacion> EmitirAsync(Guid clienteId, Guid recursoId, int horas, DateTime fechaHoraInicio, Guid? clienteIdParaPlan, CancellationToken ct = default)
    {
        var desglose = await _calculo.CalcularPrecioAsync(new ServicioCalculoPrecio.SolicitudCalculo(recursoId, horas, fechaHoraInicio, clienteIdParaPlan, null, null), ct);
        var cotizacion = Cotizacion.Emitir(Guid.NewGuid(), clienteId, recursoId, desglose.PrecioTotal, DateTime.UtcNow);
        await _repo.GuardarAsync(cotizacion, ct);

        foreach (var ev in cotizacion.DomainEvents)
            await _dispatcher.DispatchAsync(ev, ct);
        cotizacion.ClearDomainEvents();
        return cotizacion;
    }

    public async Task<Cotizacion> AceptarAsync(Guid cotizacionId, CancellationToken ct = default)
    {
        var cotizacion = await _repo.ObtenerPorIdAsync(cotizacionId, ct) ?? throw new DomainException("Cotización no encontrada.");
        cotizacion.Aceptar(DateTime.UtcNow);
        await _repo.GuardarAsync(cotizacion, ct);

        foreach (var ev in cotizacion.DomainEvents)
            await _dispatcher.DispatchAsync(ev, ct);
        cotizacion.ClearDomainEvents();
        return cotizacion;
    }

    public async Task<Cotizacion> RegistrarCaducidadAsync(Guid cotizacionId, CancellationToken ct = default)
    {
        var cotizacion = await _repo.ObtenerPorIdAsync(cotizacionId, ct) ?? throw new DomainException("Cotización no encontrada.");
        cotizacion.RegistrarCaducidad(DateTime.UtcNow);
        await _repo.GuardarAsync(cotizacion, ct);

        foreach (var ev in cotizacion.DomainEvents)
            await _dispatcher.DispatchAsync(ev, ct);
        cotizacion.ClearDomainEvents();
        return cotizacion;
    }
}
