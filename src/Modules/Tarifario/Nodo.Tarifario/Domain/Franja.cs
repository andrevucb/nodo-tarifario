namespace Nodo.Tarifario.Domain;

public sealed record Franja
{
    public TimeOnly Inicio { get; }
    public TimeOnly Fin { get; }

    public Franja(TimeOnly inicio, TimeOnly fin)
    {
        if (fin <= inicio) throw new Nodo.SharedKernel.DomainException("La franja debe tener fin posterior al inicio.");
        Inicio = inicio;
        Fin = fin;
    }

    public bool Cubre(TimeOnly hora) => hora >= Inicio && hora < Fin;
}
