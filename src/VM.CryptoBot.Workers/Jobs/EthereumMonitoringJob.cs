using Microsoft.Extensions.Hosting;
using VM.CryptoBot.Infra.Services;

namespace VM.CryptoBot.Workers.Jobs;

public class EthereumMonitoringJob(
    IEthereumService ethereumService)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var ethereumBlock = await ethereumService.GetLatestBlock();

            await Task.Delay(1000, cancellationToken);
        }
    }
}