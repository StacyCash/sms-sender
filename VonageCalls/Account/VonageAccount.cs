using Newtonsoft.Json;
using VonageCalls.Interfaces;
using VonageCalls.Models;

namespace VonageCalls.Account;

public class VonageAccount
{
    private readonly HttpClient _httpClient;
    private readonly IVonageApiKey _apiKey;

    public VonageAccount(HttpClient httpClient, IVonageApiKey apiKey)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(apiKey, nameof(apiKey));

        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<decimal> GetBalanceAsync()
    {
        (string apiKey, string apiSecret) = _apiKey.GetApiKeyAndSecret();

        var requestUri = $"https://rest.nexmo.com/account/get-balance?api_key={apiKey}&api_secret={apiSecret}";
        var response = await _httpClient.GetStringAsync(requestUri);
        var result = JsonConvert.DeserializeObject<GetBalanceResponse>(response);

        return result.Value;
    }
}