using CourierProject.Domain.Enums;
using CourierProject.Domain.ValueObjects;

namespace CourierProject.Domain.Entities;

public class Shipment
{
    #region Identità
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public Guid IdempotencyKey { get; private set; }
    #endregion

    #region Corriere
    public CarrierCode CarrierCode { get; private set; }
    public string? TrackingNumber { get; private set; }
    public string ServiceCode { get; private set; }
    public string ServiceName { get; private set; }
    public ShipmentPhase Phase { get; private set; }
    public ShipmentAnomaly? Anomaly { get; private set; }
    public DateTime? EstimatedDeliveryDate { get; private set; }
    #endregion

    #region Tariffa
    public Money? Price { get; private set; }
    #endregion

    #region Parti spedizione
    public Contact Sender { get; private set; }
    public Address SenderAddress { get; private set; }
    public Contact Recipient { get; private set; }
    public Address RecipientAddress { get; private set; }
    #endregion

    #region Polling
    public DateTime? LastPolledAt { get; private set; }
    public DateTime? LastEventAt { get; private set; }
    public bool PollingStopped { get; private set; }
    #endregion

    #region POD
    public DateTime? DeliveredAt { get; private set; }
    public string? SignedBy { get; private set; }
    #endregion

    #region Audit
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    #endregion

    #region Collezioni
    private readonly List<Package> _packages = new();
    public IReadOnlyCollection<Package> Packages => _packages.AsReadOnly();

    private readonly List<ShipmentSurcharge> _surcharges = new();
    public IReadOnlyCollection<ShipmentSurcharge> Surcharges => _surcharges.AsReadOnly();

    private readonly List<TrackingEvent> _trackingEvents = new();
    public IReadOnlyCollection<TrackingEvent> TrackingEvents => _trackingEvents.AsReadOnly();

    private readonly List<ShipmentDocument> _documents = new();
    public IReadOnlyCollection<ShipmentDocument> Documents => _documents.AsReadOnly();

    private readonly List<ShipmentCharge> _charges = new();
    public IReadOnlyCollection<ShipmentCharge> Charges => _charges.AsReadOnly();
    #endregion

    // <summary>Richiesto da EF Core per la materializzazione.</summary>

    private Shipment()
    {
        Sender = null!;
        SenderAddress = null!;
        Recipient = null!;
        RecipientAddress = null!;
        ServiceCode = null!;
        ServiceName = null!;
    }

    #region Creazione
    /// Prenota una spedizione PRIMA di chiamare il corriere (reserve-then-call).
    /// L'inserimento di questa riga è il modo in cui si prende possesso della
    /// chiave di idempotenza: il vincolo univoco su (UserId, IdempotencyKey)
    /// funziona da lock distribuito.
    ///
    /// La spedizione nasce in fase Created, senza tracking number e senza
    /// prezzo: li fornirà il corriere alla conferma.
    /// </summary>
    /// <param name="packages">Almeno un collo. Fissati da qui in poi.</param>
    /// <param name="surcharges">Supplementi richiesti. Vuoto se nessuno.</param>
    /// <param name="nowUtc">Istante corrente, fornito dall'Application
    /// tramite TimeProvider. Deve essere UTC.</param>
    public static Shipment Reserve(
    int userId,
    Guid idempotencyKey,
    CarrierCode carrierCode,
    string serviceCode,
    string serviceName,
    Contact sender,
    Address senderAddress,
    Contact recipient,
    Address recipientAddress,
    IReadOnlyCollection<Package> packages,
    IReadOnlyCollection<ShipmentSurcharge> surcharges,
    DateTime nowUtc)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId), "UserId must be positive.");

        // Guid.Empty = il client non ha mandato la chiave. Accettarla farebbe
        // collidere tutte le richieste senza chiave dello stesso utente.
        if (idempotencyKey == Guid.Empty)
            throw new ArgumentException("IdempotencyKey cannot be empty.", nameof(idempotencyKey));

        if (!Enum.IsDefined(carrierCode))
            throw new ArgumentOutOfRangeException(nameof(carrierCode), "Unknown carrier.");

        if (string.IsNullOrWhiteSpace(serviceCode))
            throw new ArgumentException("ServiceCode is required.", nameof(serviceCode));

        if (string.IsNullOrWhiteSpace(serviceName))
            throw new ArgumentException("ServiceName is required.", nameof(serviceName));

        ArgumentNullException.ThrowIfNull(sender);
        ArgumentNullException.ThrowIfNull(senderAddress);
        ArgumentNullException.ThrowIfNull(recipient);
        ArgumentNullException.ThrowIfNull(recipientAddress);
        ArgumentNullException.ThrowIfNull(packages);
        ArgumentNullException.ThrowIfNull(surcharges);

        if (packages.Count == 0)
            throw new ArgumentException("A shipment requires at least one package.", nameof(packages));

        if (packages.Any(p => p is null))
            throw new ArgumentException("Packages cannot contain null items.", nameof(packages));

        if (surcharges.Any(s => s is null))
            throw new ArgumentException("Surcharges cannot contain null items.", nameof(surcharges));

        if (surcharges.GroupBy(s => s.Type).Any(g => g.Count() > 1))
            throw new ArgumentException("Each surcharge type can be requested only once.", nameof(surcharges));

        EnsureUtc(nowUtc, nameof(nowUtc));

        var shipment = new Shipment
        {
            UserId = userId,
            IdempotencyKey = idempotencyKey,
            CarrierCode = carrierCode,
            ServiceCode = serviceCode.Trim(),
            ServiceName = serviceName.Trim(),
            Sender = sender,
            SenderAddress = senderAddress,
            Recipient = recipient,
            RecipientAddress = recipientAddress,
            Phase = ShipmentPhase.Created,
            CreatedAt = nowUtc,
            UpdatedAt = nowUtc
        };

        shipment._packages.AddRange(packages);
        shipment._surcharges.AddRange(surcharges);

        return shipment;
    }
    #endregion


    #region Helper

    private static void EnsureUtc(DateTime value, string paramName)
    {
        if (value.Kind != DateTimeKind.Utc)
            throw new ArgumentException("DateTime must be UTC.", paramName);
    }

    #endregion
}
