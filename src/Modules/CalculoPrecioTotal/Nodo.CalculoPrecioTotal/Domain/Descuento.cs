using Nodo.SharedKernel;

namespace Nodo.CalculoPrecioTotal.Domain;

public sealed record Descuento
{
    public decimal Porcentaje { get; }

    public Descuento(decimal porcentaje)
    {
        if (porcentaje < 0 || porcentaje > 100) throw new DomainException("El descuento debe estar entre 0 y 100.");
        Porcentaje = porcentaje;
    }
}
