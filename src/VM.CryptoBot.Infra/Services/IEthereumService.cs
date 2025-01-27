using Nethereum.RPC.Eth.DTOs;

namespace VM.CryptoBot.Infra.Services;

public interface IEthereumService
{
    Task<bool> CheckIfTransactionIsValidForPurchasing(Transaction transaction, TransactionReceipt? receipt);
}