namespace Nodo.SharedKernel;

/// <summary>Value Object compartido. Inmutable, igualdad por valor.</summary>
public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0) throw new DomainException("El monto no puede ser negativo.");
        if (string.IsNullOrWhiteSpace(currency)) throw new DomainException("La moneda es obligatoria.");
        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency = "BOB") => new(0, currency);

    public Money Add(Money other)
    {
        if (other.Currency != Currency) throw new DomainException("Monedas distintas.");
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (other.Currency != Currency) throw new DomainException("Monedas distintas.");
        if (other.Amount > Amount) throw new DomainException("El monto a restar no puede ser mayor al disponible.");
        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor)
    {
        if (factor < 0) throw new DomainException("El factor no puede ser negativo.");
        return new Money(Amount * factor, Currency);
    }
}
