using System.Linq.Expressions;

namespace VM.CryptoBot.Core.Table.Abstractions;

public interface IVMTableClient
{
    Task<T?> GetFilteredEntityAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken) where T : class, IVMTableItem;

    void AddEntity<T>(T entity) where T : class, IVMTableItem;

    void UpdateEntity<T>(T entity) where T : class, IVMTableItem;

    void DeleteEntity<T>(T entity) where T : class, IVMTableItem;
}