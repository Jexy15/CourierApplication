namespace CourierProject.Domain.ValueObjects;

/// <summary>
/// Peso in chilogrammi (unità canonica del dominio).
/// La conversione verso libbre o altre unità avviene solo nel gateway
/// del corriere.
/// </summary>
public sealed record Weight
{
    public decimal Kilograms { get; init; }

    public Weight(decimal kilograms)
    {
        if (kilograms <= 0)
            throw new ArgumentOutOfRangeException(nameof(kilograms), "Weight must be greater than zero.");

        Kilograms = kilograms;
    }

    /// <summary>
    /// Il maggiore fra due pesi. I corrieri fatturano sul maggiore fra
    /// peso reale e peso volumetrico.
    /// </summary>
    public static Weight Max(Weight first, Weight second)
        => first.Kilograms >= second.Kilograms ? first : second;
}