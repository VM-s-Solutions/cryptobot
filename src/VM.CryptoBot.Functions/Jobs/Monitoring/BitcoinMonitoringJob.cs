using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VM.CryptoBot.Functions.Jobs.Monitoring
{
    public class BitcoinMonitoringJob(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<BitcoinMonitoringJob>();

        [Function(nameof(BitcoinMonitoringJob))]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
