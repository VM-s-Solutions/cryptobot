namespace VM.CryptoBot.Domain.Entities.Common;

public class BaseEntity<T> : IEntity<T>
{
    public virtual T? Id { get; set; }

    public bool IsActive { get; set; } = true;

    object IEntity.Id
    {
        get => this.Id!;
        set => this.Id = (T)value;
    }
}
