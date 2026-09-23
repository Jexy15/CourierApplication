using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// <summary>Richiesto da EF Core per la materializzazione.</summary>

    private Shipment()
    {
        Sender = null!;
        SenderAddress = null!;
        Recipient = null!;
        RecipientAddress = null!;
        ServiceCode = null!;
        ServiceName = null!;
    }
}
