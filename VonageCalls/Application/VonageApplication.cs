using System.Text;
using Models.Application;
using Newtonsoft.Json;
using VonageCalls.Interfaces;

namespace VonageCalls.Application;

public class VonageApplication
{
    private HttpClient _httpClient;
    private string _apiKey;
    private string _apiSecret;

    public VonageApplication(HttpClient httpClient, IVonageApiKey apiKey)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(apiKey);

        _httpClient = httpClient;
        (_apiKey, _apiSecret) = apiKey.GetApiKeyAndSecret();
    }

    public async Task<GetApplicationsResponse> GetApplicationsAsync()
    {
        var requestUri = $"https://api.nexmo.com/v2/applications?api_key={_apiKey}&api_secret={_apiSecret}";
        var response = await _httpClient.GetStringAsync(requestUri);
        var result = JsonConvert.DeserializeObject<GetApplicationsResponse>(response);
        
        return result;
    }

    public async Task<VonageApp> GetApplicationAsync(Guid appId)
    {
        var requestUri = $"https://api.nexmo.com/v2/applications/{appId}?api_key={_apiKey}&api_secret={_apiSecret}";
        var response = await _httpClient.GetStringAsync(requestUri);
        var result = JsonConvert.DeserializeObject<VonageApp>(response);

        return result;
    }

    public async Task DeleteApplicationAsync(Guid appId)
    {
        var requestUri = $"https://api.nexmo.com/v2/applications/{appId}?api_key={_apiKey}&api_secret={_apiSecret}";
        var result = await _httpClient.DeleteAsync(requestUri);
        result.EnsureSuccessStatusCode();
    }

    public async Task<VonageApp> CreateAppAsync(VonageAppRequest request)
    {
        var requestUri = $"https://api.nexmo.com/v2/applications?api_key={_apiKey}&api_secret={_apiSecret}";
        var content = new StringContent(JsonConvert.SerializeObject(request, DefaultJsonOptions.DefaJsonSerializerSettings), Encoding.UTF8, "application/json");
        var result = await _httpClient.PostAsync(requestUri, content);
        result.EnsureSuccessStatusCode();
        
        var appJson = await result.Content.ReadAsStringAsync();
        var app = JsonConvert.DeserializeObject<VonageApp>(appJson);

        return app;
    }

    public async Task<VonageApp> UpdateAppAsync(VonageAppRequest request, Guid id)
    {
        var requestUri = $"https://api.nexmo.com/v2/applications/{id}?api_key={_apiKey}&api_secret={_apiSecret}";
        var content = new StringContent(JsonConvert.SerializeObject(request, DefaultJsonOptions.DefaJsonSerializerSettings), Encoding.UTF8, "application/json");
        var result = await _httpClient.PutAsync(requestUri, content);
        result.EnsureSuccessStatusCode();
        
        var appJson = await result.Content.ReadAsStringAsync();
        var app = JsonConvert.DeserializeObject<VonageApp>(appJson);

        return app;
    }
}