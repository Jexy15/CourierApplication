namespace CourierProject.Domain.ValueObjects;

/// <summary>
/// Dimensioni di un collo, in centimetri (unità canonica del dominio).
/// La conversione verso altre unità avviene solo nel gateway del corriere.
/// </summary>
public sealed record Dimensions
{
    public decimal LengthCm { get; init; }
    public decimal WidthCm { get; init; }
    public decimal HeightCm { get; init; }

    public Dimensions(decimal lengthCm, decimal widthCm, decimal heightCm)
    {
        if (lengthCm <= 0)
            throw new ArgumentOutOfRangeException(nameof(lengthCm), "Length must be greater than zero.");

        if (widthCm <= 0)
            throw new ArgumentOutOfRangeException(nameof(widthCm), "Width must be greater than zero.");

        if (heightCm <= 0)
            throw new ArgumentOutOfRangeException(nameof(heightCm), "Height must be greater than zero.");

        LengthCm = lengthCm;
        WidthCm = widthCm;
        HeightCm = heightCm;
    }

    /// <summary>
    /// Volume in centimetri cubi.
    /// </summary>
    public decimal VolumeCm3 => LengthCm * WidthCm * HeightCm;

    /// <summary>
    /// Peso volumetrico in kg. Il divisore varia da corriere a corriere
    /// (tipicamente 5000 o 6000 per misure in cm), quindi è il chiamante
    /// a fornirlo: il dominio non conosce le regole dei singoli corrieri.
    /// </summary>
    public Weight VolumetricWeight(decimal divisor)
    {
        if (divisor <= 0)
            throw new ArgumentOutOfRangeException(nameof(divisor), "Divisor must be greater than zero.");

        return new Weight(VolumeCm3 / divisor);
    }
}
