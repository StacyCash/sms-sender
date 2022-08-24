using VonageCalls.Interfaces;

namespace Client.Utils
{
    public class VonageSettings: IVonageApiKey
    {
        private readonly IConfiguration _configuration;
        public VonageSettings(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            _configuration = configuration;
        }
        public (string ApiKey, string ApiSecret) GetApiKeyAndSecret()
        {
            return (_configuration["ApiKey"], _configuration["ApiSecret"]);
        }
    }
}
