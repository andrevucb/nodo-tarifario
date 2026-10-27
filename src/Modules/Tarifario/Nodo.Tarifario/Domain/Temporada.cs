namespace Nodo.Tarifario.Domain;

public sealed record Temporada
{
    public DateOnly Inicio { get; }
    public DateOnly Fin { get; }

    public Temporada(DateOnly inicio, DateOnly fin)
    {
        if (fin < inicio) throw new Nodo.SharedKernel.DomainException("La temporada debe tener fin posterior o igual al inicio.");
        Inicio = inicio;
        Fin = fin;
    }

    public bool Cubre(DateOnly fecha) => fecha >= Inicio && fecha <= Fin;
}
