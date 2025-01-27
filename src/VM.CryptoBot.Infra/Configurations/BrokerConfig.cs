using Microsoft.Extensions.Configuration;
using VM.CryptoBot.Infra.Configurations.Common;
using VM.CryptoBot.Infra.Configurations.Interfaces;

namespace VM.CryptoBot.Infra.Configurations;

public class BrokerConfig(IConfiguration configuration)
    : AutoBindConfig(configuration, "Broker"), IBrokerConfig
{
    public string PrivateKey { get; init; } = default!;

    public string ApiKey { get; init; } = default!;
}