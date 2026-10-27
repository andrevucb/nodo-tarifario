using Nodo.SharedKernel;

namespace Nodo.Tarifario.Domain;

public class Tarifa : Entity<Guid>
{
    public Guid TarifaId => Id;
    public string TipoRecurso { get; private set; } = string.Empty;
    public bool EsHoraPico { get; private set; }
    public Money TarifaBase { get; private set; } = Money.Zero();
    public Franja Franja { get; private set; } = new(new TimeOnly(0,0), new TimeOnly(23,59));
    public Temporada? Temporada { get; private set; }

    private Tarifa() { }

    public Tarifa(Guid tarifaId, string tipoRecurso, bool esHoraPico, Money tarifaBase, Franja franja, Temporada? temporada = null)
    {
        Id = tarifaId == Guid.Empty ? Guid.NewGuid() : tarifaId;
        TipoRecurso = string.IsNullOrWhiteSpace(tipoRecurso) ? throw new DomainException("El tipo de recurso es obligatorio.") : tipoRecurso;
        EsHoraPico = esHoraPico;
        TarifaBase = tarifaBase ?? throw new DomainException("La tarifa base es obligatoria.");
        Franja = franja ?? throw new DomainException("La franja es obligatoria.");
        Temporada = temporada;
    }

    public void ActualizarImporte(Money nuevoImporte)
    {
        TarifaBase = nuevoImporte ?? throw new DomainException("El importe es obligatorio.");
    }
}
