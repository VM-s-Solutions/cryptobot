using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using VM.CryptoBot.Domain.Common;
using VM.CryptoBot.Infra.Services;

namespace VM.CryptoBot.AppServices.Services;

public class CoinService(IWeb3 web3) : ICoinService
{
    /// <summary>
    /// Retrieves the total supply from the ERC20 contract and converts it to a decimal based on the token's decimals.
    /// </summary>
    public async Task<decimal> GetTotalTokenSupply(string tokenAddress)
    {
        var contract = web3.Eth.GetContract(Constants.ERC20Abi, tokenAddress);

        // 1. Get token decimals
        var decimalsFunction = contract.GetFunction("decimals");
        var decimals = await decimalsFunction.CallAsync<byte>();

        // 2. Get totalSupply (raw BigInteger)
        var totalSupplyFunction = contract.GetFunction("totalSupply");
        var totalSupplyRaw = await totalSupplyFunction.CallAsync<BigInteger>();

        // 3. Convert from raw to decimal
        var divisor = (decimal)Math.Pow(10, decimals);
        return (decimal)totalSupplyRaw / divisor;
    }

    /// <summary>
    /// Sends a transaction to the ERC20 contract to transfer tokens from the broker to the recipient.
    /// </summary>
    public async Task<bool> PurchaseToken(string brokerAddress, string tokenAddress, decimal amount, string recipientAddress)
    {
        var contract = web3.Eth.GetContract(Constants.ERC20Abi, tokenAddress);

        // 1. Get token decimals
        var decimalsFunction = contract.GetFunction("decimals");
        var decimals = await decimalsFunction.CallAsync<byte>();

        // 2. Convert decimal 'amount' to raw BigInteger
        var rawAmount = ToRaw(amount, decimals);

        // 3. Prepare the 'transfer' function
        var transferFunction = contract.GetFunction("transfer");

        // 4. Send the transaction (broker must have tokens + your IWeb3 must be configured with broker's private key)
        var txHash = await transferFunction.SendTransactionAsync(
            from: brokerAddress,
            gas: new HexBigInteger(300_000),      // You can estimate or set manually
            value: null,
            functionInput:
            [
                recipientAddress,
                rawAmount
            ]
        );

        // Optionally wait for transaction to mine:
        var receipt = await WaitForReceiptAsync(txHash);
        return receipt?.Status.Value == 1;
    }

    /// <summary>
    /// Utility method to convert a user-friendly token amount to its raw BigInteger form.
    /// </summary>
    private static BigInteger ToRaw(decimal amount, byte decimals)
    {
        var multiplier = (decimal)Math.Pow(10, decimals);
        return (BigInteger)(amount * multiplier);
    }

    /// <summary>
    /// Helper to poll for a transaction receipt.
    /// </summary>
    private async Task<TransactionReceipt?> WaitForReceiptAsync(string txHash, int pollingDelayMs = 2000)
    {
        TransactionReceipt? receipt = null;
        while (receipt == null)
        {
            await Task.Delay(pollingDelayMs);
            receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
        }
        return receipt;
    }
}