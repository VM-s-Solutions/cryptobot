using System.Text.Json;
using Azure.Identity;
using Azure.Storage.Queues;
using VM.CryptoBot.Core.Queue.Abstractions;

namespace VM.CryptoBot.Infra.Storage.Queue;

public class VMQueueClient : IVMQueueClient
{
    private readonly QueueClient _queueClient;

    public VMQueueClient(string connectionString, string queueName, bool useDefaultAzureCredential = false, bool createQueueIfNotExists = false)
    {
        _queueClient = useDefaultAzureCredential
            ? new QueueClient(new Uri($"{connectionString}/{queueName}"), new DefaultAzureCredential(), new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            })
            : new QueueClient(connectionString, queueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

        if (createQueueIfNotExists)
        {
            _queueClient.CreateIfNotExists();
        }
    }
    
    public async Task SendMessageAsync<T>(T? message, CancellationToken cancellationToken) where T : IVMQueueItem
    {
        var messageString = message is null ? null : JsonSerializer.Serialize(message);
        await SendMessageAsync(messageString, cancellationToken);
    }

    public async Task SendMessageAsync(string? message, CancellationToken cancellationToken)
    {
        await _queueClient.SendMessageAsync(message, cancellationToken);
    }
}