using Nodo.SharedKernel;
using Nodo.Tarifario.Domain.Events;

namespace Nodo.Tarifario.Domain;

public class Tarifario : AggregateRoot<Guid>
{
    private readonly List<Tarifa> _tarifas = [];
    public IReadOnlyCollection<Tarifa> Tarifas => _tarifas.AsReadOnly();

    private Tarifario() { }

    public static Tarifario Crear(Guid tarifarioId)
    {
        var t = new Tarifario { Id = tarifarioId == Guid.Empty ? Guid.NewGuid() : tarifarioId };
        return t;
    }

    public void AgregarTarifa(Tarifa tarifa)
    {
        if (tarifa is null) throw new DomainException("La tarifa es obligatoria.");
        _tarifas.Add(tarifa);
    }

    public void ActualizarTarifa(Guid tarifaId, Money nuevoImporte, string solicitanteUsuarioId, string solicitanteRol)
    {
        if (!string.Equals(solicitanteRol, "Direccion", StringComparison.OrdinalIgnoreCase))
            throw new DomainException("Solo Direccion puede modificar las tarifas. (INV-01)");

        var tarifa = _tarifas.FirstOrDefault(t => t.Id == tarifaId) 
            ?? throw new DomainException("Tarifa no encontrada.");

        var importeAnterior = tarifa.TarifaBase;
        tarifa.ActualizarImporte(nuevoImporte);

        Raise(new TarifaActualizada(
            Id,
            tarifa.Id,
            tarifa.TipoRecurso,
            tarifa.EsHoraPico,
            tarifa.Franja.Inicio,
            tarifa.Franja.Fin,
            tarifa.Temporada?.Inicio,
            tarifa.Temporada?.Fin,
            importeAnterior,
            tarifa.TarifaBase,
            solicitanteUsuarioId ?? "desconocido",
            solicitanteRol,
            DateTime.UtcNow));
    }
}
