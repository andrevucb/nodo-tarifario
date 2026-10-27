using Nodo.SharedKernel;
using Nodo.CalculoPrecioTotal.Domain.Contracts;

namespace Nodo.CalculoPrecioTotal.Domain;

public class ServicioCalculoPrecio
{
    private readonly ITarifasQuery _tarifas;
    private readonly IMembresiasQuery _membresias;
    private readonly IEspaciosQuery _espacios;

    public ServicioCalculoPrecio(ITarifasQuery tarifas, IMembresiasQuery membresias, IEspaciosQuery espacios)
    {
        _tarifas = tarifas;
        _membresias = membresias;
        _espacios = espacios;
    }

    public async Task<DesglosePrecio> CalcularPrecioAsync(SolicitudCalculo solicitud, CancellationToken ct = default)
    {
        var recurso = await _espacios.ObtenerRecursoAsync(solicitud.RecursoId, ct) ?? throw new DomainException("Recurso no encontrado.");
        var plan = solicitud.ClienteId.HasValue ? await _membresias.ObtenerPlanDelMiembroAsync(solicitud.ClienteId.Value, ct) : null;
        var tarifa = await _tarifas.ObtenerTarifaAplicableAsync(recurso.TipoRecurso, solicitud.FechaHoraInicio, ct) ?? throw new DomainException("No existe tarifa aplicable.");

        var horasSolicitadas = solicitud.Horas;
        var horasIncluidas = plan?.HorasIncluidas ?? 0;
        var horasCubiertas = Math.Min(horasSolicitadas, horasIncluidas);
        var horasAdicionales = Math.Max(0, horasSolicitadas - horasIncluidas);

        var moneda = tarifa.TarifaBase.Currency;
        var importeHorasCubiertas = Money.Zero(moneda);
        var importeHorasAdicionalesBruto = tarifa.TarifaBase.Multiply(horasAdicionales);

        // INV-07: descuento del plan a las horas extra del plan de equipo (se aplica si hay plan)
        var descuentoPct = plan?.DescuentoPorcentaje ?? 0m;
        var importeHorasAdicionalesNeto = descuentoPct == 0
            ? importeHorasAdicionalesBruto
            : new Money(importeHorasAdicionalesBruto.Amount * (1 - descuentoPct / 100m), moneda);

        var precioTotal = importeHorasAdicionalesNeto; // horas cubiertas por plan suelen ser sin costo según doc
        var facturable = horasAdicionales > 0;

        return new DesglosePrecio(
            recurso.RecursoId,
            horasSolicitadas,
            horasCubiertas,
            horasAdicionales,
            importeHorasCubiertas,
            importeHorasAdicionalesBruto,
            descuentoPct,
            importeHorasAdicionalesNeto,
            precioTotal,
            facturable);
    }

    public sealed record SolicitudCalculo(Guid RecursoId, int Horas, DateTime FechaHoraInicio, Guid? ClienteId, int? Aforo, List<string>? ServiciosExtra);
}
