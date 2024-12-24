using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VM.CryptoBot.Domain.Store;

namespace VM.CryptoBot.Config.Database;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CryptoBotDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));
        services.AddScoped<IUnitOfWork>(provider => provider.GetService<CryptoBotDbContext>()!);

        return services;
    }
}