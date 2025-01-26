using VM.CryptoBot.Domain.Common.Entities;
using VM.CryptoBot.Domain.Enums;

namespace VM.CryptoBot.Domain.Coins;

public class Coin : Auditable<Guid>
{
    public string Name { get; set; }
    public BlockchainType Symbol { get; set; }
}