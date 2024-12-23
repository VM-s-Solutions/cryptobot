using Microsoft.EntityFrameworkCore;
using VM.CryptoBot.Api.Middlewares;
using VM.CryptoBot.Domain.Repositories.Common;
using VM.CryptoBot.Domain.Repositories.Interfaces;
using VM.CryptoBot.Domain.Store;
using VM.CryptoBot.Infra.Configurations;
using VM.CryptoBot.Infra.Configurations.Interfaces;

namespace VM.CryptoBot.Api.Extensions;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddMiddlewares()
            .AddRepositories()
            .AddDb(configuration)
            .AddConfigs();
    }

    private static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserSessionProvider, UserSessionProvider>();

        return services;
    }

    private static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CryptoBotDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));
        services.AddScoped<IUnitOfWork>(provider => provider.GetService<CryptoBotDbContext>()!);

        return services;
    }

    private static IServiceCollection AddConfigs(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseConnectionString, ConnectionStringConfig>();

        return services;
    }
}