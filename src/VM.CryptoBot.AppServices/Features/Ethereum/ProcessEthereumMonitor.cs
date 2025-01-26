using Nethereum.Web3;
using VM.CryptoBot.AppServices.Abstractions;
using VM.CryptoBot.Core.Queue.Abstractions;
using VM.CryptoBot.Domain.Common;
using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.AppServices.Features.Ethereum;

public class ProcessEthereumMonitor
{
    public class Command : ICommand<Guid>;

    internal class Handler(
        IWeb3 web3,
        IVMQueueFactory queueFactory)
        : ICommandHandler<Command, Guid>
    {
        public async Task<BusinessResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNumber);
            foreach (var blockTransaction in block.Transactions)
            {
                var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(blockTransaction.TransactionHash);
                if (receipt is null || receipt.Status.Value != Constants.EthereumTransaction.Success)
                {
                    var queueClient = queueFactory.GetClient(Constants.Queue.ProcessEthereumPurchase);
                    await queueClient.SendMessageAsync(blockTransaction.TransactionHash, cancellationToken);
                }
            }
            return Guid.NewGuid();
        }
    }
}