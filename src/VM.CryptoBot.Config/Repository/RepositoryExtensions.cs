using Microsoft.Extensions.DependencyInjection;
using VM.CryptoBot.Domain.Repositories.Common;
using VM.CryptoBot.Domain.Repositories.Interfaces;

namespace VM.CryptoBot.Config.Repository;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserSessionProvider, UserSessionProvider>();

        return services;
    }
}