using System.Numerics;
using Microsoft.Extensions.Configuration;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using VM.CryptoBot.Infra.Services;

namespace VM.CryptoBot.AppServices.Services;

public class AccountService(
    IWeb3 web3,
    IConfiguration configuration)
    : IAccountService
{
    public Account GetBrokerAccount()
    {
        // Load the broker account from configuration
        var privateKey = GetBrokerPrivateKey();
        return new Account(privateKey);
    }

    public string GetBrokerPrivateKey()
    {
        // Retrieve the broker's private key from configuration (or a secure store)
        return configuration["Broker:PrivateKey"];
    }

    public bool IsBalanceSufficient(string accountAddress, BigInteger amount, string contractAddress)
    {
        // Check ERC20 token balance
        var balanceTask = Task.Run(() =>
        {
            var coinService = new CoinService(web3);
            return coinService.GetBalanceAsync(accountAddress, contractAddress);
        });

        var balance = balanceTask.Result;
        return balance.Value >= amount;
    }
}