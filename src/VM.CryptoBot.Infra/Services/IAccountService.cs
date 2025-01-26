using System.Numerics;
using Nethereum.Web3.Accounts;

namespace VM.CryptoBot.Infra.Services;

public interface IAccountService
{
    Account GetBrokerAccount();
    string GetBrokerPrivateKey();
    bool IsBalanceSufficient(string accountAddress, BigInteger amount, string contractAddress);
}