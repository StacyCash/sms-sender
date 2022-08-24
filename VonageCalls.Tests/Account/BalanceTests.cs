using FluentAssertions;
using RichardSzalay.MockHttp;
using VonageCalls.Account;
using VonageCalls.Interfaces;
using VonageCalls.Tests.VonageSettingsMocks;

namespace VonageCalls.Tests.Account;

public class BalanceTests
{
    // When calling balance should call with correct account ID and secret

    [Fact]
    public async void Should_Return_Correct_Balance()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("https://rest.nexmo.com/account/get-balance?api_key=mykey&api_secret=mysecret")
            .Respond("application/json", "{\"value\" : 1.5, \"autoReload\": true }");
        var httpClient = mockHttp.ToHttpClient();

        IVonageApiKey apiKey = new MockVonageApiKey();

        // Act
        VonageAccount sut = new VonageAccount(httpClient, apiKey);
        decimal result = await sut.GetBalanceAsync();

        // Assert
        result.Should().Be(1.5m);
    }
}