using VM.CryptoBot.Domain.Common.Entities;

namespace VM.CryptoBot.Domain.Transactions;

public class ProfitTransaction : Auditable<Guid>
{
    public decimal SalePrice { get; private set; }
    public decimal ProfitAmount { get; private set; }
    public DateTime SoldAt { get; private set; }
    public Guid PurchaseTransactionId { get; private set; }
    public virtual PurchaseTransaction? PurchaseTransaction { get; private set; }
}