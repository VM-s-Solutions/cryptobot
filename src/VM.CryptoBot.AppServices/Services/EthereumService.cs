using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using VM.CryptoBot.Domain.Common;
using VM.CryptoBot.Infra.Configurations.Interfaces;
using VM.CryptoBot.Infra.Services;

namespace VM.CryptoBot.AppServices.Services;

public class EthereumService(
    IBotConfig botSettings,
    ICoinService coinService)
    : IEthereumService
{
    public async Task<bool> CheckIfTransactionIsValidForPurchasing(Transaction transaction, TransactionReceipt? receipt)
    {
        if (receipt is not null || receipt?.Status.Value == Constants.EthereumTransaction.Success)
        {
            return false;
        }

        var isPercentageValid = await MeetsPercentageRequirementAsync(transaction, botSettings.TransactionPercentage);
        if (!isPercentageValid)
        {
            return false;
        }

        return await OvercomeFeeWithinLimitAsync(transaction, botSettings.MaxOvercomeAmount);
    }

    private async Task<bool> MeetsPercentageRequirementAsync(
        Transaction transaction,
        decimal requiredPercentage)
    {
        if (string.IsNullOrEmpty(transaction.To))
        {
            return false;
        }

        // Get total supply of the token from the contract
        var totalSupply = await coinService.GetTotalTokenSupply(transaction.To);

        // Convert Wei to Ether (or the token's base unit as needed)
        var transactionValue = Web3.Convert.FromWei(transaction.Value.Value);

        // Calculate the percentage
        var transactionPercentage = (transactionValue / totalSupply) * 100m;

        return transactionPercentage >= requiredPercentage;
    }

    private static async Task<bool> OvercomeFeeWithinLimitAsync(Transaction transaction, decimal maxOvercomeAmount)
    {
        var transactionValue = Web3.Convert.FromWei(transaction.Value.Value);

        // Example: Overcome fee is 1% of the transaction value
        var overcomeFee = transactionValue * 0.01m;

        // Check if 2x the fee is under the maximum allowed threshold
        return await Task.FromResult(overcomeFee * 2 <= maxOvercomeAmount);
    }
}