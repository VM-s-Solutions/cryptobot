using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VM.CryptoBot.Core.Table.Abstractions;

namespace VM.CryptoBot.Infra.Storage.Table;

public static class VMTableExtensions
{
    public static IServiceCollection AddAzureTable(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddSingleton<IVMTableFactory, VMTableFactory>(provider => new VMTableFactory(configuration.GetConnectionString("AzureStorageTable")!, env.IsProduction(), env.IsDevelopment()));
        return services;
    }
}