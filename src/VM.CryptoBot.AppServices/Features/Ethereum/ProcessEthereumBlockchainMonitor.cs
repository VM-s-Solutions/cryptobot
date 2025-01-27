using Nethereum.Web3;
using VM.CryptoBot.AppServices.Abstractions;
using VM.CryptoBot.Core.Queue.Abstractions;
using VM.CryptoBot.Domain.Common;
using VM.CryptoBot.Infra.Services;
using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.AppServices.Features.Ethereum;

public class ProcessEthereumBlockchainMonitor
{
    public class Command : ICommand<Guid>;

    internal class Handler(
        IWeb3 web3,
        IVMQueueFactory queueFactory,
        IEthereumService ethereumService)
        : ICommandHandler<Command, Guid>
    {
        public async Task<BusinessResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockNumber);
            foreach (var transaction in block.Transactions)
            {
                var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction.TransactionHash);
                if (await ethereumService.CheckIfTransactionIsValidForPurchasing(transaction, receipt))
                {
                    var queueClient = queueFactory.GetClient(Constants.Queue.ProcessEthereumPurchase);
                    await queueClient.SendMessageAsync(transaction.TransactionHash, cancellationToken);
                }
            }
            return Guid.NewGuid();
        }
    }
}