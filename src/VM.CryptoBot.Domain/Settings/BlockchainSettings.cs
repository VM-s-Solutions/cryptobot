using VM.CryptoBot.Domain.Common.Entities;
using VM.CryptoBot.Domain.Enums;

namespace VM.CryptoBot.Domain.Settings;

public class BlockchainSettings : Auditable<Guid>
{
    public BlockchainType BlockchainName { get; private set; }
    public string ApiEndpoint { get; private set; }
    public decimal ProfitThreshold { get; private set; }
    public decimal TransactionExpirationInSeconds { get; private set; }
    public decimal OvercomingAmount { get; private set; }
}
