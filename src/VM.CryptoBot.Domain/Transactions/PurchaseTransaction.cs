using VM.CryptoBot.Domain.Coins;
using VM.CryptoBot.Domain.Common.Entities;
using VM.CryptoBot.Domain.Enums;

namespace VM.CryptoBot.Domain.Transactions;

public class PurchaseTransaction : Auditable<Guid>
{
    public string TransactionHash { get; private set; }
    public BlockchainType BlockchainType { get; private set; }
    public Guid CoinId { get; private set; }
    public Coin Coin { get; private set; }
    public decimal Amount { get; private set; }
    public decimal Price { get; private set; }
    public decimal TotalCost { get; private set; }
    public TransactionStatus Status { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public Guid? ProfitTransactionId { get; private set; }
    public virtual ProfitTransaction? ProfitTransaction { get; private set; }

    public static PurchaseTransaction Create(string transactionHash, BlockchainType blockchainType, decimal amount, decimal price)
    => new()
    {
        TransactionHash = transactionHash,
        BlockchainType = blockchainType,
        Amount = amount,
        Price = price,
        TotalCost = amount * price,
        Status = TransactionStatus.Pending
    };
}