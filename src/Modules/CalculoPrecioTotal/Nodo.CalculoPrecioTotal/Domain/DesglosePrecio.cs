using Nodo.SharedKernel;

namespace Nodo.CalculoPrecioTotal.Domain;

public sealed record DesglosePrecio(
    Guid RecursoId,
    int HorasSolicitadas,
    int HorasCubiertas,
    int HorasAdicionales,
    Money ImporteHorasCubiertas,
    Money ImporteHorasAdicionalesBruto,
    decimal DescuentoAplicadoPorcentaje,
    Money ImporteHorasAdicionalesNeto,
    Money PrecioTotal,
    bool Facturable);
