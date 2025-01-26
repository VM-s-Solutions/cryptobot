namespace VM.CryptoBot.Domain.Common.Entities;

public class BaseEntity<T> : IEntity<T>
{
    public virtual T? Id { get; set; }

    public bool IsActive { get; set; } = true;

    object IEntity.Id
    {
        get => Id!;
        set => Id = (T)value;
    }
}
