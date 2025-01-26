using Microsoft.Extensions.Hosting;
using VM.CryptoBot.Infra.Services;

namespace VM.CryptoBot.Workers.Jobs;

public class BitcoinMonitoringJob(
    IBitcoinService bitcoinService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var bitcoinBlock = await bitcoinService.GetLatestBlock();

            await Task.Delay(1000, cancellationToken);
        }
    }
}