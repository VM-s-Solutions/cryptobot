using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.RPC.Accounts;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using VM.CryptoBot.Infra.Configurations;
using VM.CryptoBot.Infra.Configurations.Interfaces;

namespace VM.CryptoBot.Config.Settings;

public static class SettingExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddSingleton<IBotConfig, BotConfig>();
        services.AddSingleton<IBrokerConfig, BrokerConfig>();
        services.AddSingleton<IDatabaseConnectionString, ConnectionStringConfig>();

        return services;
    }

    public static IServiceCollection AddWeb3(this IServiceCollection services)
    {
        services.AddSingleton<IAccount, Account>(provider =>
        {
            var brokerConfig = provider.GetRequiredService<IBrokerConfig>();
            return new Account(brokerConfig.PrivateKey, Chain.Sepolia);
        });
        services.AddSingleton<IWeb3, Web3>(provider =>
        {
            var brokerConfig = provider.GetRequiredService<IBrokerConfig>();
            var account = provider.GetRequiredService<IAccount>();
            return new Web3(account, $"https://eth-sepolia.g.alchemy.com/v2/{brokerConfig.ApiKey}");
        });

        return services;
    }
}