using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VM.CryptoBot.Domain.Common;

namespace VM.CryptoBot.Functions.Jobs.Transactions
{
    public class ProcessEthereumPurchaseJob(
        ILogger<ProcessEthereumPurchaseJob> logger)
    {
        [Function(nameof(ProcessEthereumPurchaseJob))]
        public void Run([QueueTrigger(Constants.Queue.ProcessEthereumPurchase, Connection = "")] QueueMessage message)
        {
            logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
