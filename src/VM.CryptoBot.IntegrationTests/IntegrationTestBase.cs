using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using VM.CryptoBot.Config;
using VM.CryptoBot.Domain.Store;
using VM.CryptoBot.TestUtilities;

namespace VM.CryptoBot.IntegrationTests;

public class IntegrationTestBase : BaseTransactionalPostgresSqlTest<CryptoBotDbContext>
{
    public readonly IConfiguration Configuration;

    protected IntegrationTestBase()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.IntegrationTests.json")
#if DEBUG
            .AddJsonFile("appsettings.IntegrationTests.Development.json")
#endif
            .Build();
    }

    protected override async Task TestMethod<TResult>(
        Func<IServiceCollection, Task>? setup = null,
        Func<CryptoBotDbContext, Task>? arrange = null,
        Func<IServiceProvider, Task<TResult>>? act = null,
        Func<CryptoBotDbContext, TResult, Task>? assert = null,
        Func<CryptoBotDbContext, Task>? cleanup = null,
        bool transactional = true)
    {
        await base.TestMethod(Setup, arrange, act, assert, cleanup, transactional);
        return;

        async Task Setup(IServiceCollection services)
        {
            services.AddLogging(builder => { builder.AddConsole(); });
            services.AddCoreBindings(Configuration, new HostingEnvironment());

            if (setup is not null)
            {
                await setup(services);
            }
        }
    }
}