using CourierProject.Domain.Enums;
using CourierProject.Domain.ValueObjects;

namespace CourierProject.Domain.Entities;

/// <summary>
/// Singolo collo di una spedizione. Una spedizione può averne più di uno
/// (multicollo).
/// Nota: il tracking è a livello di Shipment (master tracking number).
/// TrackingNumber qui è il numero del singolo collo, se il corriere lo
/// restituisce, ma non porta stato.
/// </summary>
public class Package
{
    public int Id { get; private set; }
    public int ShipmentId { get; private set; }

    public PackageType PackageType { get; private set; }
    public Weight Weight { get; private set; }
    public Dimensions Dimensions { get; private set; }
    public string? ContentDescription { get; private set; }

    /// <summary>Assegnato dal corriere alla creazione della spedizione.</summary>
    public string? TrackingNumber { get; private set; }

    /// <summary>Richiesto da EF Core per la materializzazione.</summary>
    private Package()
    {
        Weight = null!;
        Dimensions = null!;
    }

    public Package(
        PackageType packageType,
        Weight weight,
        Dimensions dimensions,
        string? contentDescription = null)
    {
        PackageType = packageType;
        Weight = weight ?? throw new ArgumentNullException(nameof(weight));
        Dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
        ContentDescription = string.IsNullOrWhiteSpace(contentDescription)
            ? null
            : contentDescription.Trim();
    }

    /// <summary>
    /// Peso fatturabile: i corrieri applicano il maggiore fra peso reale
    /// e peso volumetrico. Il divisore varia per corriere, quindi arriva
    /// dal gateway.
    /// </summary>
    public Weight BillableWeight(decimal volumetricDivisor)
        => Weight.Max(Weight, Dimensions.VolumetricWeight(volumetricDivisor));

    /// <summary>
    /// Assegna il tracking number restituito dal corriere.
    /// Operazione una-tantum: un collo già tracciato non cambia numero.
    /// </summary>
    public void AssignTrackingNumber(string trackingNumber)
    {
        if (string.IsNullOrWhiteSpace(trackingNumber))
            throw new ArgumentException("TrackingNumber is required.", nameof(trackingNumber));

        if (TrackingNumber is not null)
            throw new InvalidOperationException("TrackingNumber is already assigned.");

        TrackingNumber = trackingNumber.Trim();
    }
}
