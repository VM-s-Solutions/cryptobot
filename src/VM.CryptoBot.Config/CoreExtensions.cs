using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VM.CryptoBot.Config.Database;
using VM.CryptoBot.Config.Repository;
using VM.CryptoBot.Config.Settings;

namespace VM.CryptoBot.Config;

public static class CoreExtensions
{
    public static IServiceCollection AddCoreBindings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddSettings(configuration);
        services.AddDatabase(configuration);

        return services;
    }
}