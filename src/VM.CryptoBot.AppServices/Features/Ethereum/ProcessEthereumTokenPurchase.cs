using FluentValidation;
using MediatR;
using Nethereum.RPC.Accounts;
using Nethereum.Web3;
using VM.CryptoBot.Infra.Configurations.Interfaces;
using VM.CryptoBot.Infra.Services;
using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.AppServices.Features.Ethereum;

public class ProcessEthereumTokenPurchase
{
    public record Command(string TransactionHash) : IRequest<BusinessResult<string>>;

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(cmd => cmd.TransactionHash)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Transaction hash is required.")
                .Must(hash => hash.StartsWith("0x") && hash.Length == 66);
        }
    }

    internal class Handler(
        IWeb3 web3,
        IAccount account,
        ICoinService coinService)
        : IRequestHandler<Command, BusinessResult<string>>
    {
        public async Task<BusinessResult<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(request.TransactionHash);

                // Recompute the necessary purchase amount
                var transactionValue = Web3.Convert.FromWei(transaction.Value.Value);
                var overcomeFee = transactionValue * 0.01m;
                var purchaseAmount = transactionValue + overcomeFee;

                // Attempt the token purchase
                // Replace with your actual token contract address:
                var targetTokenAddress = "0x69A316B81b00a2fCc881390116deC6DC498ad950";
                var purchaseSuccess = await coinService.PurchaseToken(account.Address, transaction.To, purchaseAmount, targetTokenAddress);

                return !purchaseSuccess
                    ? BusinessResult.Failure<string>(Error.Create(nameof(purchaseSuccess), "Failed to execute token purchase."))
                    : BusinessResult.Success("Token purchase executed successfully.");
            }
            catch (Exception ex)
            {
                return BusinessResult.Failure<string>(Error.Create(nameof(Exception), $"Error processing token purchase: {ex.Message}"));
            }
        }
    }
}