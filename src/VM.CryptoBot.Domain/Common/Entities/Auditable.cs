namespace VM.CryptoBot.Domain.Common.Entities;

public class Auditable<T> : BaseEntity<T>
{
    public string CreatedBy { get; private set; } = default!;

    public DateTimeOffset CreatedAt { get; private set; }

    public string? UpdatedBy { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    public string? DeactivatedBy { get; private set; }

    public DateTimeOffset? DeactivatedAt { get; private set; }

    public Auditable<T> Created(string createdBy, DateTimeOffset? createdAt = null)
    {
        CreatedBy = createdBy;
        CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
        return this;
    }

    public Auditable<T> Updated(string updatedBy, DateTimeOffset? updatedAt = null)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt ?? DateTimeOffset.Now;
        return this;
    }

    public Auditable<T> Deactivated(string deactivatedBy, DateTimeOffset? deactivatedAt = null)
    {
        DeactivatedBy = deactivatedBy;
        DeactivatedAt = deactivatedAt ?? DateTimeOffset.Now;
        return this;
    }
}