using Newtonsoft.Json;

namespace Models.Application;

public class Webhooks
{
    [JsonProperty("inbound_url")]
    public VonageUrl InboundUrl { get; set; }

    [JsonProperty("status_url")]
    public VonageUrl StatusUrl { get; set; }
}