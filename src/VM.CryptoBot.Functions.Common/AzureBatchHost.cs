using System.Reflection;
using Microsoft.Extensions.Hosting;
using VM.CryptoBot.Config;

namespace VM.CryptoBot.Functions.Common;

public static class AzureBatchHost
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var assembly = Assembly.GetEntryAssembly();

        var directory = Directory.GetCurrentDirectory();
        if (assembly is not null)
        {
            directory = Path.GetDirectoryName(assembly.Location);
        }

        if (directory is not null)
        {
            return Host.CreateDefaultBuilder(args)
                .UseContentRoot(directory)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddCoreBindings(hostContext.Configuration, hostContext.HostingEnvironment);
                })
                .ConfigureFunctionsWorkerDefaults();
        }

        throw new ArgumentException("Could not configure HostBuilder without content root.");
    }

    public static async Task<int> RunAsync(string[] args, Func<IHostBuilder, IHostBuilder> extend, Func<IHost, Task> execute)
    {
        var host = extend(CreateHostBuilder(args))
            .Build();

        using (host)
        {
            await execute(host);
        }

        return Environment.ExitCode;
    }

    public static async Task<int> RunAsync(string[] args, Func<IHostBuilder, IHostBuilder> extend)
    {
        return await RunAsync(args, extend, host => host.RunAsync());
    }

    public static async Task<int> RunAsync(string[] args)
    {
        return await RunAsync(args, host => host);
    }
}