using System.Linq.Expressions;
using VM.CryptoBot.Domain.Entities.Common;
using VM.CryptoBot.Domain.Store;

namespace VM.CryptoBot.Domain.Repositories.Interfaces;

public interface IRepository<TEntity, in TKey> : IUnitOfWork
        where TEntity : IEntity<TKey>
{
    Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken);

    Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken);

    IQueryable<TEntity> GetByIds(TKey[] ids);

    IQueryable<TEntity> GetPaged(int offset, int limit, Expression<Func<TEntity, bool>> filter);

    //IQueryable<TEntity> GetPagedSort<TSort>(int offset, int limit, Expression<Func<TEntity, bool>> filter, SortDefinition sort)
    //    where TSort : BaseSort<TEntity>;

    //IQueryable<TEntity> GetPagedSort<TSort>(int offset, int limit, Expression<Func<TEntity, bool>> filter, IEnumerable<SortDefinition> sort)
    //    where TSort : BaseSort<TEntity>;

    Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken);

    IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);

    IQueryable<TEntity> GetAll();

    void Attach(TEntity entity);

    void AttachRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void Deactivate(TEntity entity);

    void DeactivateRange(IEnumerable<TEntity> entities);
}
