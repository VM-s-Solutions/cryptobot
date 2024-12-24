using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace VM.CryptoBot.TestUtilities;

public class BaseMockedDbContextTest<TDbContext> where TDbContext : DbContext
{
    protected virtual async Task TestMethod<TResult>(
        Action<IServiceCollection>? setup = null,
        Action<Mock<TDbContext>>? arrange = null,
        Func<IServiceProvider, Task<TResult>>? act = null,
        Action<Mock<TDbContext>, TResult>? assert = null,
        Func<Mock<TDbContext>, Task>? cleanup = null)
    {
        await TestMethod(
            setup: setup,
            arrange: arrange,
            act: act,
            assert: (context, result) =>
            {
                assert?.Invoke(context, result);
                return Task.CompletedTask;
            },
            cleanup);
    }

    protected virtual async Task TestMethod<TResult>(
        Action<IServiceCollection>? setup = null,
        Action<Mock<TDbContext>>? arrange = null,
        Func<IServiceProvider, Task<TResult>>? act = null,
        Func<Mock<TDbContext>, TResult, Task>? assert = null,
        Func<Mock<TDbContext>, Task>? cleanup = null)
    {
        await TestMethod(
            setup: setup,
            arrange: context =>
            {
                arrange?.Invoke(context);
                return Task.CompletedTask;
            },
            act: act,
            assert: assert,
            cleanup);
    }

    protected virtual async Task TestMethod<TResult>(
        Action<IServiceCollection>? setup = null,
        Func<Mock<TDbContext>, Task>? arrange = null,
        Func<IServiceProvider, Task<TResult>>? act = null,
        Func<Mock<TDbContext>, TResult, Task>? assert = null,
        Func<Mock<TDbContext>, Task>? cleanup = null)
    {
        await TestMethod(
            setup: services =>
            {
                setup?.Invoke(services);
                return Task.CompletedTask;
            },
            arrange: arrange,
            act: act,
            assert: assert,
            cleanup);
    }

    protected virtual async Task TestMethod<TResult>(
        Action<IServiceCollection>? setup = null,
        Func<Mock<TDbContext>, Task>? arrange = null,
        Func<IServiceProvider, TResult>? act = null,
        Func<Mock<TDbContext>, TResult, Task>? assert = null,
        Func<Mock<TDbContext>, Task>? cleanup = null)
    {
        await TestMethod(
            setup: services =>
            {
                setup?.Invoke(services);
                return Task.CompletedTask;
            },
            arrange: arrange,
            act: provider =>
            {
                TResult? result = default;
                if (act != null)
                {
                    result = act(provider);
                }

                return Task.FromResult(result)!;
            },
            assert: assert,
            cleanup);
    }

    protected virtual async Task TestMethod<TResult>(
        Action<IServiceCollection>? setup = null,
        Action<Mock<TDbContext>>? arrange = null,
        Func<IServiceProvider, TResult>? act = null,
        Action<Mock<TDbContext>, TResult?>? assert = null,
        Func<Mock<TDbContext>, Task>? cleanup = null)
    {
        await TestMethod(
            setup: setup,
            arrange: async context =>
            {
                arrange?.Invoke(context);
                await Task.CompletedTask;
            },
            act: provider =>
            {
                TResult? result = default;
                if (act != null)
                {
                    result = act(provider);
                }

                return Task.FromResult(result);
            },
            assert: (context, result) =>
            {
                assert?.Invoke(context, result);
                return Task.CompletedTask;
            },
            cleanup);
    }

    protected virtual async Task TestMethod<TResult>(
        Func<IServiceCollection, Task>? setup = null,
        Func<Mock<TDbContext>, Task>? arrange = null,
        Func<IServiceProvider, Task<TResult>>? act = null,
        Func<Mock<TDbContext>, TResult, Task>? assert = null,
        Func<Mock<TDbContext>, Task>? cleanup = null)
    {
        Func<IServiceProvider, Task>? arr = null;
        if (arrange != null)
        {
            arr = provider =>
            {
                var dbContext = provider.GetService<Mock<TDbContext>>()!;

                return arrange(dbContext);
            };
        }

        await TestMethod(setup, arr, act, assert, cleanup);
    }

    protected virtual async Task TestMethod<TResult>(
        Func<IServiceCollection, Task>? setup = null,
        Func<IServiceProvider, Task>? arrange = null,
        Func<IServiceProvider, Task<TResult>>? act = null,
        Func<Mock<TDbContext>, TResult, Task>? assert = null,
        Func<Mock<TDbContext>, Task>? cleanup = null)
    {
        if (typeof(Task).IsAssignableFrom(typeof(TResult)))
        {
            throw new Exception("""
                                'TResult' cannot be of type 'Task'.
                                                                   If you see this warning, your unit test is probably wrong.
                                                                   The compiler can't determine the type of the result you return in the 'act'.
                                                                   To solve this issue, make sure you return something in the 'act' and that you assert on existing property of 'TResult'.
                                """);
        }

        TResult? result = default;

        var services = new ServiceCollection();

        if (setup is not null)
        {
            await setup(services);
        }

        var context = new Mock<TDbContext>();
        services.AddTransient(_ => context.Object);
        services.AddTransient(_ => context);

        var provider = services.BuildServiceProvider();

        try
        {
            if (arrange is not null)
            {
                using var scope = provider.CreateScope();
                await arrange(scope.ServiceProvider);
            }

            if (act is not null)
            {
                using var scope = provider.CreateScope();
                result = await act(scope.ServiceProvider);
            }

            if (assert is not null)
            {
                using var scope = provider.CreateScope();
                var dbContext = scope.ServiceProvider.GetService<Mock<TDbContext>>()!;
                await assert(dbContext, result!);
            }
        }
        finally
        {
            if (cleanup is not null)
            {
                using var scope = provider.CreateScope();
                var dbContext = scope.ServiceProvider.GetService<Mock<TDbContext>>()!;
                await cleanup(dbContext);
            }
        }
    }
}