using CourierProject.Domain.Enums;

namespace CourierProject.Domain.Entities;

/// <summary>
/// Metadati di un documento associato alla spedizione (etichetta, POD,
/// proforma). Il file binario NON sta nel database: vive su disco, fuori
/// da wwwroot, e viene scaricato tramite un endpoint che verifica la
/// proprietà della spedizione.
///
/// Immutabile: un documento emesso non cambia. Se il corriere ne emette
/// uno nuovo, è un nuovo record.
/// </summary>
public class ShipmentDocument
{
    public int Id { get; private set; }
    public int ShipmentId { get; private set; }
    public DocumentType Type { get; private set; }
    public DocumentFormat Format { get; private set; }

    /// <summary>Nome leggibile, usato per il download (Content-Disposition).</summary>
    public string FileName { get; private set; }

    /// <summary>Nome del file su disco. Generato dallo storage (GUID +
    /// estensione), mai derivato dal nome originale: evita collisioni e
    /// path traversal.</summary>
    public string StoredFileName { get; private set; }

    public long SizeBytes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    /// <summary>Richiesto da EF Core per la materializzazione.</summary>
    private ShipmentDocument()
    {
        FileName = null!;
        StoredFileName = null!;
    }

    public ShipmentDocument(
        DocumentType type,
        DocumentFormat format,
        string fileName,
        string storedFileName,
        long sizeBytes,
        DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("FileName is required.", nameof(fileName));

        if (string.IsNullOrWhiteSpace(storedFileName))
            throw new ArgumentException("StoredFileName is required.", nameof(storedFileName));

        if (storedFileName.Contains('/') || storedFileName.Contains('\\') || storedFileName.Contains(".."))
            throw new ArgumentException("StoredFileName must be a plain file name, not a path.", nameof(storedFileName));

        if (sizeBytes <= 0)
            throw new ArgumentOutOfRangeException(nameof(sizeBytes), "SizeBytes must be greater than zero.");

        Type = type;
        Format = format;
        FileName = fileName.Trim();
        StoredFileName = storedFileName.Trim();
        SizeBytes = sizeBytes;
        CreatedAt = createdAtUtc;
    }
}