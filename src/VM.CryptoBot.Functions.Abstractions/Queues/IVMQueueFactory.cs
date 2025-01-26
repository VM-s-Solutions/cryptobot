namespace VM.CryptoBot.Functions.Abstractions.Queues;

public interface IVMQueueFactory
{
    IVMQueueClient GetClient(string queue);
}