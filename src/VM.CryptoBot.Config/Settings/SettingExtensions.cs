using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VM.CryptoBot.Infra.Configurations;
using VM.CryptoBot.Infra.Configurations.Interfaces;

namespace VM.CryptoBot.Config.Settings;

public static class SettingExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDatabaseConnectionString, ConnectionStringConfig>();

        return services;
    }
}