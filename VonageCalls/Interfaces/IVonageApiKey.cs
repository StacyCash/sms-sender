namespace VonageCalls.Interfaces;

public interface IVonageApiKey
{
    (string ApiKey, string ApiSecret) GetApiKeyAndSecret();
}