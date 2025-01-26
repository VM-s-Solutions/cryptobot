using System.Numerics;
using Nethereum.Hex.HexTypes;

namespace VM.CryptoBot.Infra.Services;

public interface ICoinService
{
    Task<bool> IsValidCoinAsync(string contractAddress);
    Task<HexBigInteger> GetBalanceAsync(string accountAddress, string contractAddress);
    Task<string> TransferAsync(string senderAddress, string privateKey, string recipientAddress, BigInteger amount, string contractAddress, HexBigInteger gasPrice, HexBigInteger gasLimit);
}