using Microsoft.Extensions.Configuration;

namespace VM.CryptoBot.Infra.Configurations.Common;

public class AutoBindConfig
{
    public AutoBindConfig(IConfiguration configuration, string configurationName)
    {
        configuration.GetSection(configurationName).Bind(this);
    }
}
