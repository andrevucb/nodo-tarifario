using Nodo.SharedKernel;

namespace Nodo.Cotizaciones.Domain;

public sealed record Vigencia
{
    public DateTime Emision { get; init; }
    public DateTime Vencimiento { get; init; }

    private Vigencia() { }

    public Vigencia(DateTime emision, DateTime vencimiento)
    {
        if (vencimiento < emision) throw new DomainException("La fecha de vencimiento no puede ser anterior a la emisión.");
        Emision = emision;
        Vencimiento = vencimiento;
    }

    public static Vigencia Crear(DateTime emision, int diasVigencia = 4)
    {
        if (diasVigencia <= 0) throw new DomainException("Los días de vigencia deben ser mayores a 0.");
        return new Vigencia(emision, emision.AddDays(diasVigencia));
    }

    public bool EstaVigente(DateTime ahora) => ahora <= Vencimiento;
}
