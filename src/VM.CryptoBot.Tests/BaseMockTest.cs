using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using VM.CryptoBot.Config;
using VM.CryptoBot.Domain.Store;
using VM.CryptoBot.TestUtilities;

namespace VM.CryptoBot.Tests;

public class BaseMockTest : BaseMockedDbContextTest<CryptoBotDbContext>
{
    public readonly IConfiguration Configuration;

    protected BaseMockTest()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Tests.json")
#if DEBUG
            .AddJsonFile("appsettings.Tests.Development.json")
#endif
            .Build();
    }

    protected override async Task TestMethod<TResult>(
        Func<IServiceCollection, Task>? setup = null,
        Func<Mock<CryptoBotDbContext>, Task>? arrange = null,
        Func<IServiceProvider, Task<TResult>>? act = null,
        Func<Mock<CryptoBotDbContext>, TResult, Task>? assert = null,
        Func<Mock<CryptoBotDbContext>, Task>? cleanup = null)
    {
        await base.TestMethod(Setup, arrange, act, assert, cleanup);
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