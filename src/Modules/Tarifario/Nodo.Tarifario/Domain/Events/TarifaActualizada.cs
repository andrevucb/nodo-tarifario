using Nodo.SharedKernel;

namespace Nodo.Tarifario.Domain.Events;

public sealed record TarifaActualizada(
    Guid TarifarioId,
    Guid TarifaId,
    string TipoRecurso,
    bool EsHoraPico,
    TimeOnly FranjaInicio,
    TimeOnly FranjaFin,
    DateOnly? TemporadaInicio,
    DateOnly? TemporadaFin,
    Money ImporteAnterior,
    Money ImporteNuevo,
    string ActualizadoPorUsuarioId,
    string ActualizadoPorRol,
    DateTime OccurredOn) : IDomainEvent;
