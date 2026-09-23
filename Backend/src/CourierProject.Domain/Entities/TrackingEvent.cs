namespace CourierProject.Domain.Entities;

/// <summary>
/// Evento di tracking così come ricevuto dal corriere.
/// Conserva SEMPRE il dato grezzo (codice e descrizione originali):
/// la normalizzazione avviene sugli assi dello Shipment, qui non si
/// perde nulla.
/// Immutabile una volta creato: un evento accaduto non cambia.
/// </summary>
public class TrackingEvent
{
    public int Id { get; private set; }
    public int ShipmentId { get; private set; }

    /// <summary>Quando l'evento è accaduto, secondo il corriere. UTC.
    /// È l'ordinamento autorevole: gli eventi arrivano fuori ordine.</summary>
    public DateTime Timestamp { get; private set; }

    /// <summary>Codice originale del corriere (es. "IT", "AR", "DL").</summary>
    public string RawStatusCode { get; private set; }

    /// <summary>Descrizione originale del corriere.</summary>
    public string? RawDescription { get; private set; }

    public string? LocationCity { get; private set; }
    public string? LocationCountryCode { get; private set; }

    /// <summary>Quando NOI abbiamo acquisito l'evento. UTC.
    /// Distinto da Timestamp: un evento di ieri può arrivare oggi.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Richiesto da EF Core per la materializzazione.</summary>
    private TrackingEvent()
    {
        RawStatusCode = null!;
    }

    public TrackingEvent(
        DateTime timestampUtc,
        string rawStatusCode,
        DateTime createdAtUtc,
        string? rawDescription = null,
        string? locationCity = null,
        string? locationCountryCode = null)
    {
        if (string.IsNullOrWhiteSpace(rawStatusCode))
            throw new ArgumentException("RawStatusCode is required.", nameof(rawStatusCode));

        Timestamp = timestampUtc;
        RawStatusCode = rawStatusCode.Trim().ToUpperInvariant();
        CreatedAt = createdAtUtc;

        RawDescription = Normalize(rawDescription);
        LocationCity = Normalize(locationCity);
        LocationCountryCode = Normalize(locationCountryCode)?.ToUpperInvariant();
    }

    private static string? Normalize(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}