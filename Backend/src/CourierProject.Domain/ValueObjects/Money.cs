namespace CourierProject.Domain.ValueObjects;

/// <summary>
/// Importo monetario. Value object immutabile.
/// Importo e valuta sono sempre accoppiati: un numero senza valuta non
/// ha significato quando si confrontano tariffe di corrieri diversi.
/// Nessuna conversione fra valute è tentata.
/// </summary>
public sealed record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency) || currency.Trim().Length != 3)
            throw new ArgumentException("Currency must be a 3-letter ISO 4217 code.", nameof(currency));

        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
    }

    /// <summary>
    /// Somma due importi. Valute diverse non sono sommabili senza
    /// un tasso di cambio, che è fuori dallo scope del dominio.
    /// </summary>
    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (!Currency.Equals(other.Currency, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Cannot operate on different currencies: {Currency} and {other.Currency}.");
    }
}
