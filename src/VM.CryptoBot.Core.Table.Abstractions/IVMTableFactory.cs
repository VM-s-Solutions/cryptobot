namespace VM.CryptoBot.Core.Table.Abstractions;

public interface IVMTableFactory
{
    IVMTableClient GetClient(string queue);
}