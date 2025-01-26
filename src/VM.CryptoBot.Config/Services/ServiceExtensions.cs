using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VM.CryptoBot.AppServices.Services;
using VM.CryptoBot.Infra.Services;

namespace VM.CryptoBot.Config.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBitcoinService, BitcoinService>();
        services.AddScoped<IEthereumService, EthereumService>();

        return services;
    }
}