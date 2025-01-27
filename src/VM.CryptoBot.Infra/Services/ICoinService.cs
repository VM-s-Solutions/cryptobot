using Nethereum.Web3.Accounts;

namespace VM.CryptoBot.Infra.Services;

public interface ICoinService
{
    Task<decimal> GetTotalTokenSupply(string tokenAddress);
    Task<bool> PurchaseToken(string brokerAddress, string tokenAddress, decimal amount, string recipientAddress);
}