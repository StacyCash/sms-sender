using System.Text.Json.Serialization;

namespace VonageCalls.Models;

public class SendMessageRequest
{
    [JsonPropertyName("message_type")]
    public string MessageType { get; set; }

    public string Text { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Channel { get; set; }

}