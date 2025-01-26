namespace VM.CryptoBot.Core.Queue.Abstractions;

public interface IVMQueueClient
{
    Task SendMessageAsync<T>(T? message, CancellationToken cancellationToken) where T : IVMQueueItem;

    Task SendMessageAsync(string message, CancellationToken cancellationToken);
}