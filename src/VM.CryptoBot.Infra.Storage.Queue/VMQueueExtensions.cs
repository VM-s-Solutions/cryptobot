using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VM.CryptoBot.Core.Queue.Abstractions;

namespace VM.CryptoBot.Infra.Storage.Queue;

public static class VMQueueExtensions
{
    public static IServiceCollection AddAzureQueue(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddSingleton<IVMQueueFactory, VMQueueFactory>(provider => new VMQueueFactory(configuration.GetConnectionString("AzureStorageQueue")!, env.IsProduction(), env.IsDevelopment()));
        return services;
    }
}