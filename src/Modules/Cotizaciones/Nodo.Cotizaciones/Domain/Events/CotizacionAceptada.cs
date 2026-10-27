using Nodo.SharedKernel;

namespace Nodo.Cotizaciones.Domain.Events;

public sealed record CotizacionAceptada(Guid CotizacionId, Guid ClienteId, Guid RecursoId, Money PrecioOfrecido, DateTime AceptadaEn) : IDomainEvent;
