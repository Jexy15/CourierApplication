using CourierProject.Domain.Enums;
using CourierProject.Domain.ValueObjects;

namespace CourierProject.Domain.Entities;

/// <summary>
/// Voce di prezzo così come restituita dal corriere: tariffa base,
/// carburante, costo dell'assicurazione, zona remota, sconti...
/// Rappresenta il CONTO, non l'intenzione.
///
/// Mapping IN ENTRATA, come gli stati di tracking: il codice grezzo è
/// sempre conservato, il tipo canonico è valorizzato solo se il gateway
/// riconosce il codice. Un codice sconosciuto non rompe niente: resta
/// con Type = null e la descrizione originale.
///
/// Immutabile: una voce fatturata non cambia.
/// </summary>
public class ShipmentCharge
{
    public int Id { get; private set; }
    public int ShipmentId { get; private set; }

    /// <summary>Codice originale del corriere.</summary>
    public string RawCode { get; private set; }

    /// <summary>Descrizione originale del corriere.</summary>
    public string? RawDescription { get; private set; }

    /// <summary>Tipo canonico. Null se il codice non è riconosciuto.</summary>
    public ChargeType? Type { get; private set; }

    /// <summary>Importo della voce, sempre positivo.
    /// Per gli sconti il segno è dato dal tipo (Discount), non dal numero.</summary>
    public Money Amount { get; private set; }

    /// <summary>Richiesto da EF Core per la materializzazione.</summary>
    private ShipmentCharge()
    {
        RawCode = null!;
        Amount = null!;
    }

    public ShipmentCharge(
        string rawCode,
        Money amount,
        ChargeType? type = null,
        string? rawDescription = null)
    {
        if (string.IsNullOrWhiteSpace(rawCode))
            throw new ArgumentException("RawCode is required.", nameof(rawCode));

        RawCode = rawCode.Trim();
        Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        Type = type;
        RawDescription = string.IsNullOrWhiteSpace(rawDescription) ? null : rawDescription.Trim();
    }
}
