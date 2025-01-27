using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VM.CryptoBot.AppServices.Features.Ethereum;
using VM.CryptoBot.Domain.Common;

namespace VM.CryptoBot.Functions.Jobs.Transactions;

public class ProcessEthereumTokenPurchaseJob(
    IMediator mediator,
    ILogger<ProcessEthereumTokenPurchaseJob> logger)
{
    [Function(nameof(ProcessEthereumTokenPurchaseJob))]
    public async Task Run([QueueTrigger(Constants.Queue.ProcessEthereumPurchase, Connection = "AzureStorageQueue")] string message, CancellationToken cancellationToken)
    {
        logger.LogInformation($"C# Queue trigger function processed: {message}");
        await mediator.Send(new ProcessEthereumTokenPurchase.Command(message), cancellationToken);
    }
}