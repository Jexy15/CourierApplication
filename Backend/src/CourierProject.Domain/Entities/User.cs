namespace CourierProject.Domain.Entities;

/// <summary>
/// Utente del sistema. Store minimale.
/// </summary>
public class User
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    /// <summary>Richiesto da EF Core per la materializzazione.</summary>
    private User()
    {
        Email = null!;
        PasswordHash = null!;
    }

    public User(string email, string passwordHash, DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("PasswordHash is required.", nameof(passwordHash));

        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        CreatedAt = createdAtUtc;
    }
}
