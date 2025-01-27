namespace VM.CryptoBot.Infra.Configurations.Interfaces;

public interface IBrokerConfig
{
    string PrivateKey { get; init; }

    string ApiKey { get; init; }
}