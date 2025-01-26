using VM.CryptoBot.Core.Table.Abstractions;

namespace VM.CryptoBot.Infra.Storage.Table;

public class VMTableFactory(string connectionString, bool isProduction, bool isDevelopment) : IVMTableFactory
{
    public IVMTableClient GetClient(string queue)
    {
        return new VMTableClient(connectionString, queue, isProduction, isDevelopment);
    }
}