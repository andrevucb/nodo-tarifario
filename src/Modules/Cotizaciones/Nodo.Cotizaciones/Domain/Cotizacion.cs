using Nodo.SharedKernel;
using Nodo.Cotizaciones.Domain.Events;

namespace Nodo.Cotizaciones.Domain;

public class Cotizacion : AggregateRoot<Guid>
{
    public Guid CotizacionId => Id;
    public Guid ClienteId { get; private set; }
    public Guid RecursoId { get; private set; }
    public Money PrecioOfrecido { get; private set; } = Money.Zero();
    public Vigencia Vigencia { get; private set; } = null!;
    public EstadoCotizacion Estado { get; private set; } = EstadoCotizacion.Emitida;

    private Cotizacion() { }

    public static Cotizacion Emitir(Guid cotizacionId, Guid clienteId, Guid recursoId, Money precio, DateTime fechaEmision)
    {
        if (clienteId == Guid.Empty) throw new DomainException("ClienteId es obligatorio.");
        if (recursoId == Guid.Empty) throw new DomainException("RecursoId es obligatorio.");
        if (precio is null) throw new DomainException("El precio ofrecido es obligatorio.");

        var c = new Cotizacion
        {
            Id = cotizacionId == Guid.Empty ? Guid.NewGuid() : cotizacionId,
            ClienteId = clienteId,
            RecursoId = recursoId,
            PrecioOfrecido = precio,
            Vigencia = Vigencia.Crear(fechaEmision, 4),
            Estado = EstadoCotizacion.Emitida
        };

        c.Raise(new CotizacionEmitida(c.Id, c.ClienteId, c.RecursoId, c.PrecioOfrecido, c.Vigencia.Emision, c.Vigencia.Vencimiento));
        return c;
    }

    public void Aceptar(DateTime ahora)
    {
        if (Estado != EstadoCotizacion.Emitida)
            throw new DomainException("Solo se puede aceptar una cotización en estado Emitida. (INV-05)");

        if (!Vigencia.EstaVigente(ahora))
            throw new DomainException("No se puede aceptar una cotización caducada. (INV-05)");

        Estado = EstadoCotizacion.Aceptada;
        Raise(new CotizacionAceptada(Id, ClienteId, RecursoId, PrecioOfrecido, ahora));
    }

    public void RegistrarCaducidad(DateTime ahora)
    {
        if (Estado != EstadoCotizacion.Emitida)
            return;

        if (Vigencia.EstaVigente(ahora))
            throw new DomainException("La cotización aún está vigente, no puede marcarse como caducada.");

        Estado = EstadoCotizacion.Caducada;
        Raise(new CotizacionCaducada(Id, ahora));
    }
}
