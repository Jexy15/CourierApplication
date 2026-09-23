namespace CourierProject.Domain.Enums;

/// <summary>
/// Tipo canonico di una voce di prezzo restituita dal corriere.
/// Le voci con codice non riconosciuto hanno tipo null e conservano
/// codice e descrizione originali.
/// </summary>
public enum ChargeType
{
    Base = 1,       // Tariffa base del servizio
    Fuel = 2,       // Supplemento carburante, applicato dal corriere
    Insurance = 3,  // Costo dell'assicurazione richiesta
    COD = 4,        // Costo del servizio di contrassegno
    Discount = 5    // Sconto: importo positivo, il segno lo dà il tipo
}