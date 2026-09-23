using CourierProject.Domain.Enums;

namespace CourierProject.Domain.ValueObjects;

/// <summary>
/// Indirizzo postale. Value object: due indirizzi con gli stessi valori
/// sono lo stesso indirizzo. Immutabile.
/// La validazione qui garantisce solo la FORMA minima; i requisiti
/// specifici del corriere sono validati dal gateway.
/// </summary>
public sealed record Address
{
    public string Street { get; init; }
    public string? StreetNumber { get; init; }
    public AddressNumberType? NumberType { get; init; }
    public string? AdditionalInfo { get; init; }
    public string City { get; init; }
    public string PostalCode { get; init; }
    public string? Province { get; init; }
    public string? Region { get; init; }
    public string CountryCode { get; init; }

    public Address(
        string street,
        string city,
        string postalCode,
        string countryCode,
        string? streetNumber = null,
        AddressNumberType? numberType = null,
        string? additionalInfo = null,
        string? province = null,
        string? region = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street is required.", nameof(street));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.", nameof(city));

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("PostalCode is required.", nameof(postalCode));

        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Trim().Length != 2)
            throw new ArgumentException("CountryCode must be a 2-letter ISO 3166-1 code.", nameof(countryCode));

        Street = street.Trim();
        City = city.Trim();
        PostalCode = postalCode.Trim();
        CountryCode = countryCode.Trim().ToUpperInvariant();

        StreetNumber = Normalize(streetNumber);
        NumberType = numberType;
        AdditionalInfo = Normalize(additionalInfo);
        Province = Normalize(province);
        Region = Normalize(region);
    }

    private static string? Normalize(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
