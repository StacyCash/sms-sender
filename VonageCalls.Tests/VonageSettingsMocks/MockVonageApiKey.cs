using VonageCalls.Interfaces;

namespace VonageCalls.Tests.VonageSettingsMocks;

internal class MockVonageApiKey : IVonageApiKey
{
    public (string ApiKey, string ApiSecret) GetApiKeyAndSecret()
    {
        return ("mykey", "mysecret");
    }
}