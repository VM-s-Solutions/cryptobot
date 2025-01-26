using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VM.CryptoBot.Config.Database;
using VM.CryptoBot.Config.Repository;
using VM.CryptoBot.Config.Services;
using VM.CryptoBot.Config.Settings;
using VM.CryptoBot.Infra.Storage.Queue;
using VM.CryptoBot.Infra.Storage.Table;

namespace VM.CryptoBot.Config;

public static class CoreExtensions
{
    public static IServiceCollection AddCoreBindings(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddRepositories();
        services.AddSettings(configuration);
        services.AddDatabase(configuration);
        services.AddServices(configuration);
        services.AddAzureQueue(configuration, env);
        services.AddAzureTable(configuration, env);

        return services;
    }
}