using Microsoft.Extensions.Configuration;
using VM.CryptoBot.Infra.Configurations.Common;
using VM.CryptoBot.Infra.Configurations.Interfaces;

namespace VM.CryptoBot.Infra.Configurations;

public class BotConfig(IConfiguration configuration)
    : AutoBindConfig(configuration, "Bot"), IBotConfig
{
    public decimal TransactionPercentage { get; init; } = default;
    public decimal MaxOvercomeAmount { get; init; } = default;
}