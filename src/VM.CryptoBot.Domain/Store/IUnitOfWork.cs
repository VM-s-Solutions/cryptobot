namespace VM.CryptoBot.Domain.Store;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    void Rollback();
}