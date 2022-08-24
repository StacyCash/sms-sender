using FluentAssertions;
using Models.Application;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using VonageCalls.Interfaces;
using VonageCalls.Tests.VonageSettingsMocks;
using VonageCalls.Application;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VonageCalls.Tests.Applications;

public class GetApplicationTests
{
    [Fact]
    public async void Should_Return_List_Of_Applications()
    {
        // Arrange
        string jsonResponse;
        using (StreamReader r = new StreamReader("Applications\\GetApplicationsResponse.json"))
        {
            jsonResponse = r.ReadToEnd();
        }

        GetApplicationsResponse expected = JsonConvert.DeserializeObject<GetApplicationsResponse>(jsonResponse);

        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("https://api.nexmo.com/v2/applications?api_key=mykey&api_secret=mysecret")
            .Respond("application/json", jsonResponse);
        var httpClient = mockHttp.ToHttpClient();

        IVonageApiKey apiKey = new MockVonageApiKey();

        VonageApplication sut = new VonageApplication(httpClient, apiKey);

        // Act
        GetApplicationsResponse result = await sut.GetApplicationsAsync();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void Should_Return_Single_Application()
    {
        // Arrange
        string jsonResponse;
        using (StreamReader r = new StreamReader("Applications\\GetApplicationResponse.json"))
        {
            jsonResponse = r.ReadToEnd();
        }

        VonageApp expected = JsonConvert.DeserializeObject<VonageApp>(jsonResponse);

        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("https://api.nexmo.com/v2/applications/a01e3929-38a4-4db0-aa7c-429ccbe95176?api_key=mykey&api_secret=mysecret")
            .Respond("application/json", jsonResponse);
        var httpClient = mockHttp.ToHttpClient();

        IVonageApiKey apiKey = new MockVonageApiKey();

        VonageApplication sut = new VonageApplication(httpClient, apiKey);

        Guid appId = Guid.Parse("a01e3929-38a4-4db0-aa7c-429ccbe95176");

        // Act
        VonageApp result = await sut.GetApplicationAsync(appId);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}