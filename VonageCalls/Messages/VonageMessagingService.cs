using System.Net.Http.Json;
using Models.Messages;
using VonageCalls.Enums;
using VonageCalls.Models;

namespace VonageCalls.Messages;

public class VonageMessagingService
{
    private readonly HttpClient _httpClient;

    public VonageMessagingService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    public async Task SendMessage(SmsMessage message)
    {
        SendMessageRequest payload = new SendMessageRequest
        {
            MessageType = MessageType.Text.ToString().ToLower(),
            To = GetPayloadTelNumber(message.To),
            From = GetPayloadTelNumber(message.From),
            Channel = ChannelType.Sms.ToString().ToLower(),
            Text = message.Message
        };

        var requestUri = "https://api.nexmo.com/v1/messages";
        var result = await _httpClient.PostAsJsonAsync(requestUri, payload);
        result.EnsureSuccessStatusCode();
    }

    private string GetPayloadTelNumber(string telNumber)
    {
        if (telNumber.Substring(0, 2) == "00")
        {
            return telNumber.Substring(2);
        }

        return telNumber.Replace("+", String.Empty);
    }
}