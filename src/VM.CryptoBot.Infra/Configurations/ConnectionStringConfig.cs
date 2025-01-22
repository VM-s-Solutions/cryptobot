using Microsoft.Extensions.Configuration;
using VM.CryptoBot.Infra.Configurations.Common;
using VM.CryptoBot.Infra.Configurations.Interfaces;

namespace VM.CryptoBot.Infra.Configurations;

public class ConnectionStringConfig(IConfiguration configuration)
    : AutoBindConfig(configuration, "ConnectionStrings"), IDatabaseConnectionString
{
    public string ConnectionString { get; set; } = default!;
}