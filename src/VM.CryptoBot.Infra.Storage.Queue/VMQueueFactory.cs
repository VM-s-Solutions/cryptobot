using VM.CryptoBot.Core.Queue.Abstractions;

namespace VM.CryptoBot.Infra.Storage.Queue;

public class VMQueueFactory(string connectionString, bool isProduction, bool isDevelopment) : IVMQueueFactory
{
    public IVMQueueClient GetClient(string queue)
    {
        return new VMQueueClient(connectionString, queue, isProduction, isDevelopment);
    }
}