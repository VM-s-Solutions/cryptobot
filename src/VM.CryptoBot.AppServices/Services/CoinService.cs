using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using VM.CryptoBot.Domain.Common;
using VM.CryptoBot.Infra.Services;

namespace VM.CryptoBot.AppServices.Services;

public class CoinService(
    IWeb3 web3)
    : ICoinService
{
    public async Task<bool> IsValidCoinAsync(string contractAddress)
    {
        try
        {
            // Check if the contract implements the ERC20 "totalSupply" function
            var contract = web3.Eth.GetContract(Constants.ERC20Abi, contractAddress);
            var totalSupplyFunction = contract.GetFunction("totalSupply");
            var totalSupply = await totalSupplyFunction.CallAsync<BigInteger>();
            return totalSupply >= 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<HexBigInteger> GetBalanceAsync(string accountAddress, string contractAddress)
    {
        var contract = web3.Eth.GetContract(Constants.ERC20Abi, contractAddress);
        var balanceFunction = contract.GetFunction("balanceOf");
        return await balanceFunction.CallAsync<HexBigInteger>(accountAddress);
    }

    public async Task<string> TransferAsync(
        string senderAddress,
        string privateKey,
        string recipientAddress,
        BigInteger amount,
        string contractAddress,
        HexBigInteger gasPrice,
        HexBigInteger gasLimit)
    {
        var contract = web3.Eth.GetContract(Constants.ERC20Abi, contractAddress);
        var transferFunction = contract.GetFunction("transfer");

        // Sign the transaction
        var transactionInput = transferFunction.CreateTransactionInput(senderAddress, gasPrice, gasLimit, null, recipientAddress, amount);
        var transaction = await web3.TransactionManager.SignTransactionAsync(transactionInput);

        // Send the transaction
        return await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(transaction);
    }
}