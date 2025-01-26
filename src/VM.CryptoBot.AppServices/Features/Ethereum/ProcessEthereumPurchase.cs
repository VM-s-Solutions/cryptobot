using FluentValidation;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using VM.CryptoBot.AppServices.Abstractions;
using VM.CryptoBot.Domain.Common;
using VM.CryptoBot.Infra.Services;
using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.AppServices.Features.Ethereum;

public class ProcessEthereumPurchase
{
    public record Command(string TransactionHash) : ICommand<Guid>;

    internal class Validator : AbstractValidator<Command>
    {
        private readonly IWeb3 _web3;

        public Validator(IWeb3 web3)
        {
            _web3 = web3 ?? throw new ArgumentNullException(nameof(web3));

            RuleFor(command => command.TransactionHash)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Transaction hash is required.")
                .WithErrorCode(nameof(Command.TransactionHash))
                .MustAsync(IsValidTransactionHashAsync)
                .WithMessage("Transaction not found or invalid.")
                .WithErrorCode(nameof(Command.TransactionHash));

            RuleFor(command => command.TransactionHash)
                .MustAsync(IsRecipientAContractAsync)
                .WithMessage("Recipient is not a valid contract address.")
                .WithErrorCode(nameof(Command.TransactionHash));
        }

        private async Task<bool> IsValidTransactionHashAsync(string transactionHash, CancellationToken cancellationToken)
        {
            var transaction = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionHash);
            return transaction != null;
        }

        private async Task<bool> IsRecipientAContractAsync(string transactionHash, CancellationToken cancellationToken)
        {
            var transaction = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionHash);
            if (transaction == null)
            {
                return false;
            }

            var recipientCode = await _web3.Eth.GetCode.SendRequestAsync(transaction.To);
            return !string.IsNullOrEmpty(recipientCode) && recipientCode != "0x";
        }
    }

    internal class Handler(
        IWeb3 web3,
        IAccountService accountService,
        ICoinService coinService) : ICommandHandler<Command, Guid>
    {
        public async Task<BusinessResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            // Retrieve the transaction details using the hash
            var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(request.TransactionHash);

            // Extract the relevant details (e.g., recipient, value, etc.)
            var recipient = transaction.To;
            var value = transaction.Value; // In Wei
            var gasPrice = transaction.GasPrice; // Current gas price for the transaction

            // Use the broker account for sending the purchase
            var brokerAccount = accountService.GetBrokerAccount();
            var brokerAddress = brokerAccount.Address;

            // Calculate an increased gas price to prioritize the transaction
            var newGasPrice = gasPrice.Value + (gasPrice.Value / 2); // 50% higher gas price

            // Define the amount to purchase
            var purchaseAmount = value.Value / 2; // Purchase half the seen transaction amount

            // Interact with the ERC20 token contract (assuming it's a coin)
            var tokenContract = web3.Eth.GetContract(Constants.ERC20Abi, recipient);

            // Get the transfer function
            var transferFunction = tokenContract.GetFunction("transfer");

            // Estimate gas for the purchase transaction
            var gasLimit = await transferFunction.EstimateGasAsync(brokerAddress, new HexBigInteger(purchaseAmount), null, brokerAddress, purchaseAmount);

            // Create the transaction for purchasing the coin
            var transactionInput = transferFunction.CreateTransactionInput(brokerAddress, new HexBigInteger(newGasPrice), gasLimit, null, brokerAddress, purchaseAmount);

            // Send the transaction
            var transactionHash = await web3.Eth.Transactions.SendTransaction.SendRequestAsync(transactionInput);

            // Return the transaction hash as the result
            return BusinessResult.Success(new Guid(transactionHash[..32]));
        }
    }
}