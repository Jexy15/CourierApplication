using CourierProject.Domain.Enums;
using CourierProject.Domain.ValueObjects;

namespace CourierProject.Domain.Entities;

/// <summary>
/// Supplemento RICHIESTO dal cliente. Rappresenta l'intenzione, non il conto:
/// viene scelto prima di GetRates e inviato al corriere come input del
/// calcolo tariffa.
///
/// Il costo non sta qui: lo restituisce il corriere e vive in ShipmentCharge,
/// insieme a tutte le altre voci di prezzo. Tenerlo in entrambi i posti
/// porterebbe al doppio conteggio.
///
/// Ogni tipo ha il suo factory method: le combinazioni sbagliate
/// (un'assicurazione senza valore, un contrassegno con valore assicurato)
/// sono impossibili da costruire.
/// </summary>
public class ShipmentSurcharge
{
    public int Id { get; private set; }
    public int ShipmentId { get; private set; }
    public SurchargeType Type { get; private set; }

    /// <summary>Solo per Insurance.</summary>
    public Money? InsuredValue { get; private set; }

    /// <summary>Solo per COD: importo da incassare alla consegna.</summary>
    public Money? CodAmount { get; private set; }

    /// <summary>Solo per COD.</summary>
    public CodPaymentMethod? CodPaymentMethod { get; private set; }

    /// <summary>Richiesto da EF Core per la materializzazione.</summary>
    private ShipmentSurcharge() { }

    private ShipmentSurcharge(SurchargeType type)
    {
        Type = type;
    }

    public static ShipmentSurcharge Insurance(Money insuredValue)
    {
        ArgumentNullException.ThrowIfNull(insuredValue);

        if (insuredValue.Amount <= 0)
            throw new ArgumentException("Insured value must be greater than zero.", nameof(insuredValue));

        return new ShipmentSurcharge(SurchargeType.Insurance)
        {
            InsuredValue = insuredValue
        };
    }

    public static ShipmentSurcharge CashOnDelivery(Money amount, CodPaymentMethod paymentMethod)
    {
        ArgumentNullException.ThrowIfNull(amount);

        if (amount.Amount <= 0)
            throw new ArgumentException("COD amount must be greater than zero.", nameof(amount));

        return new ShipmentSurcharge(SurchargeType.COD)
        {
            CodAmount = amount,
            CodPaymentMethod = paymentMethod
        };
    }
}
