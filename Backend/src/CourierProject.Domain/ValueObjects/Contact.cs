namespace CourierProject.Domain.ValueObjects;

/// <summary>
/// Riferimento di contatto (persona o ragione sociale) associato a un
/// indirizzo di spedizione. Value object immutabile.
/// Sulla spedizione è uno SNAPSHOT: copiato al momento della creazione,
/// non collegato alla rubrica.
/// </summary>
public sealed record Contact
{
    public string Name { get; init; }
    public string Phone { get; init; }
    public string? Email { get; init; }

    public Contact(string name, string phone, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required.", nameof(phone));

        Name = name.Trim();
        Phone = phone.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
    }
}