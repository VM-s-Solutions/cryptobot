using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VM.CryptoBot.AppServices.Features.Ethereum;

namespace VM.CryptoBot.Functions.Jobs.Monitoring;

public class EthereumBlockchainMonitoringJob(ILoggerFactory loggerFactory, IMediator mediator)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<EthereumBlockchainMonitoringJob>();

    [Function(nameof(EthereumBlockchainMonitoringJob))]
    public async Task Run([TimerTrigger("%EthereumBlockchainMonitoringJobSchedule%")] TimerInfo myTimer, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        await mediator.Send(new ProcessEthereumBlockchainMonitor.Command(), cancellationToken);
    }
}