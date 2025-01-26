using System.Linq.Expressions;
using Azure.Data.Tables;
using Azure.Identity;
using VM.CryptoBot.Core.Table.Abstractions;

namespace VM.CryptoBot.Infra.Storage.Table;

public class VMTableClient : IVMTableClient
{
    private readonly TableClient _tableClient;

    public VMTableClient(string connectionString, string tableName, bool useDefaultAzureCredential = false, bool createTableIfNotExists = false)
    {
        _tableClient = useDefaultAzureCredential
            ? new TableClient(new Uri($"{connectionString}/{tableName}"), tableName, new DefaultAzureCredential())
            : new TableClient(connectionString, tableName);

        if (createTableIfNotExists)
        {
            _tableClient.CreateIfNotExists();
        }
    }

    public async Task<T?> GetFilteredEntityAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken) where T : class,  IVMTableItem
    {
        var query = _tableClient.QueryAsync<T>(filter, maxPerPage: 1, cancellationToken: cancellationToken);
        await foreach (var page in query.AsPages().WithCancellation(cancellationToken))
        {
            return page.Values.FirstOrDefault();
        }

        return null;
    }

    public void AddEntity<T>(T entity) where T : class, IVMTableItem
    {
        _tableClient.AddEntity(entity);
    }

    public void UpdateEntity<T>(T entity) where T : class, IVMTableItem
    {
        _tableClient.UpdateEntity(entity, entity.ETag);
    }

    public void DeleteEntity<T>(T entity) where T : class, IVMTableItem
    {
        _tableClient.DeleteEntity(entity.PartitionKey, entity.RowKey, entity.ETag);
    }
}