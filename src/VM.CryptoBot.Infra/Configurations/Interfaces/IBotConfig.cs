namespace VM.CryptoBot.Infra.Configurations.Interfaces;

public interface IBotConfig
{
    decimal TransactionPercentage { get; init; }

    decimal MaxOvercomeAmount { get; init; }
}