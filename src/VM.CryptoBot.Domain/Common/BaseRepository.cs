using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VM.CryptoBot.Domain.Common.Entities;
using VM.CryptoBot.Domain.Repositories;
using VM.CryptoBot.Domain.Store;

namespace VM.CryptoBot.Domain.Common;

public abstract class BaseRepository<TEntity, TKey>(CryptoBotDbContext context) : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public CryptoBotDbContext Context { get; private set; } = context;

    public Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken)
    {
        return GetDbSet().AnyAsync(entity => entity.Id!.Equals(id), cancellationToken);
    }

    public Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken)
    {
        var query = GetQueryable();
        return query.FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken)!;
    }

    public IQueryable<TEntity> GetByIds(TKey[] ids)
    {
        ids = ids.Distinct().ToArray();
        var query = GetQueryable();
        return query.Where(x => ids.Contains(x.Id));
    }

    public IQueryable<TEntity> GetPaged(int offset, int limit, Expression<Func<TEntity, bool>> filter)
    {
        var query = FilterData(filter);
        return query.Skip(offset).Take(limit);
    }

    //public IQueryable<TEntity> GetPagedSort<TSort>(int offset, int limit, Expression<Func<TEntity, bool>> filter, SortDefinition sort)
    //    where TSort : BaseSort<TEntity>
    //{
    //    var query = FilterData(filter);

    //    var sortField = sort.Field?.ToLower();
    //    var sortByAsc = sort?.Direction is null or SortDirection.Ascending;
    //    var entitySort = Activator.CreateInstance(typeof(TSort), sortField, sortByAsc) as TSort;
    //    query = entitySort!.ApplySort(query);

    //    return query.Skip(offset).Take(limit);
    //}

    //public IQueryable<TEntity> GetPagedSort<TSort>(int offset, int limit, Expression<Func<TEntity, bool>> filter, IEnumerable<SortDefinition> sortDefinitions)
    //    where TSort : BaseSort<TEntity>
    //{
    //    var query = FilterData(filter);

    //    foreach (var (sort, index) in sortDefinitions.Select((value, i) => (value, i)))
    //    {
    //        var sortField = sort.Field?.ToLower();
    //        var sortByAsc = sort?.Direction is null or SortDirection.Ascending;
    //        var entitySort = Activator.CreateInstance(typeof(TSort), sortField, sortByAsc) as TSort;
    //        query = entitySort!.ApplySort(query, index != 0);
    //    }

    //    return query.Skip(offset).Take(limit);
    //}


    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken)
    {
        var query = FilterData(filter);
        return await query.CountAsync(cancellationToken);
    }

    public IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
    {
        var query = FilterData(filter);
        return query;
    }

    public IQueryable<TEntity> GetAll()
    {
        return GetQueryable();
    }

    public virtual void Attach(TEntity entity)
    {
        Context.Attach(entity);
    }

    public void AttachRange(IEnumerable<TEntity> entities)
    {
        Context.AttachRange(entities);
    }

    public virtual void Deactivate(TEntity entity)
    {
        entity.IsActive = false;
    }

    public virtual void DeactivateRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity is BaseEntity<Guid>)
            {
                entity.IsActive = false;
            }
        }
    }

    public virtual void Remove(TEntity entity)
    {
        Context.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        GetDbSet().RemoveRange(entities);
    }

    protected virtual IQueryable<TEntity> GetQueryable()
    {
        return GetDbSet();
    }

    protected abstract DbSet<TEntity> GetDbSet();

    protected IQueryable<TEntity> FilterData(Expression<Func<TEntity, bool>>? filter)
    {
        var query = GetQueryable();
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await Context.CommitAsync(cancellationToken);
    }

    public void Rollback()
    {
        Context.Rollback();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}