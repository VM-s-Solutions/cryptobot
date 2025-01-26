namespace VM.CryptoBot.Core.Queue.Abstractions;

public interface IVMQueueFactory
{
    IVMQueueClient GetClient(string queue);
}