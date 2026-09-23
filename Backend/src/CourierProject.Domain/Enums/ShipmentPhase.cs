namespace CourierProject.Domain.Enums;
/// <summary>
// Asse 1  - fase della spedizione
/// </summary>
public enum ShipmentPhase
{
    Created = 1, // Riga prenotata, corriere non ancora chiamat
    LabelCreated = 2, // Etichetta emessa, nessun evento fisico
    PickedUp = 3, // Corriere ha materialmente il pacco
    InTransit = 4, // In movimento nella rete
    InCustoms = 5, // In sdoganamento
    OutForDelivery = 6, // In consegna
    ReadyForPickup = 7, // Fermo in punto di ritiro, attende il destinatario
    Delivered = 8, // Consegnato
    ReturnToSender = 9, // In viaggio verso il mittente
    Failed = 10, //Chiusa senza consegna in modo definitivo
    Unknown = 11 // Corriere non fornisce informazioni util
}