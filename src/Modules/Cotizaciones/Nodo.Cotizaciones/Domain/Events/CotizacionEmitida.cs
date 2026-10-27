using Nodo.SharedKernel;

namespace Nodo.Cotizaciones.Domain.Events;

public sealed record CotizacionEmitida(Guid CotizacionId, Guid ClienteId, Guid RecursoId, Money PrecioOfrecido, DateTime Emision, DateTime Vencimiento) : IDomainEvent;
