using Nodo.SharedKernel;

namespace Nodo.Cotizaciones.Domain.Events;

public sealed record CotizacionCaducada(Guid CotizacionId, DateTime CaducadaEn) : IDomainEvent;
