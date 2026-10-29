using Nodo.Cotizaciones.Domain.Events;
using Nodo.SharedKernel;
using Nodo.Tarifario.Domain.Events;

namespace Nodo.Web.Infrastructure;

public sealed record DomainEventToast(string Title, string Message);

public sealed class DomainEventToastNotifications
{
    private readonly List<DomainEventToast> _notifications = [];

    public IReadOnlyList<DomainEventToast> Notifications => _notifications;

    public void Add(string title, string message) => _notifications.Add(new DomainEventToast(title, message));
}

public sealed class CotizacionEmitidaToastHandler(DomainEventToastNotifications notifications)
    : IDomainEventHandler<CotizacionEmitida>
{
    public Task HandleAsync(CotizacionEmitida domainEvent, CancellationToken ct = default)
    {
        notifications.Add("Evento: Cotización emitida",
            $"Cotización {domainEvent.CotizacionId.ToString()[..8]} por {domainEvent.PrecioOfrecido.Amount:0.00} {domainEvent.PrecioOfrecido.Currency}.");
        return Task.CompletedTask;
    }
}

public sealed class CotizacionAceptadaToastHandler(DomainEventToastNotifications notifications)
    : IDomainEventHandler<CotizacionAceptada>
{
    public Task HandleAsync(CotizacionAceptada domainEvent, CancellationToken ct = default)
    {
        notifications.Add("Evento: Cotización aceptada",
            $"Cotización {domainEvent.CotizacionId.ToString()[..8]} por {domainEvent.PrecioOfrecido.Amount:0.00} {domainEvent.PrecioOfrecido.Currency}.");
        return Task.CompletedTask;
    }
}

public sealed class CotizacionCaducadaToastHandler(DomainEventToastNotifications notifications)
    : IDomainEventHandler<CotizacionCaducada>
{
    public Task HandleAsync(CotizacionCaducada domainEvent, CancellationToken ct = default)
    {
        notifications.Add("Evento: Cotización caducada",
            $"Cotización {domainEvent.CotizacionId.ToString()[..8]}.");
        return Task.CompletedTask;
    }
}

public sealed class TarifaActualizadaToastHandler(DomainEventToastNotifications notifications)
    : IDomainEventHandler<TarifaActualizada>
{
    public Task HandleAsync(TarifaActualizada domainEvent, CancellationToken ct = default)
    {
        var tipoFranja = domainEvent.EsHoraPico ? "hora pico" : "horario normal";
        notifications.Add("Evento: Tarifa actualizada",
            $"{domainEvent.TipoRecurso}, {tipoFranja}: {domainEvent.ImporteAnterior.Amount:0.00} → {domainEvent.ImporteNuevo.Amount:0.00} {domainEvent.ImporteNuevo.Currency}.");
        return Task.CompletedTask;
    }
}