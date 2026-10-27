using Nodo.SharedKernel;

namespace Nodo.CalculoPrecioTotal.Domain;

public sealed record Horas
{
    public int Cantidad { get; }

    public Horas(int cantidad)
    {
        if (cantidad < 0) throw new DomainException("Las horas no pueden ser negativas.");
        Cantidad = cantidad;
    }
}
