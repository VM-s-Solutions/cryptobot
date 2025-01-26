namespace VM.CryptoBot.Functions.Abstractions.Queues;

public interface IVMQueueClient
{
    Task SendMessageAsync<T>(T? message, CancellationToken cancellationToken) where T : IVMQueueItem;

    Task SendMessageAsync(string? message, CancellationToken cancellationToken);
}