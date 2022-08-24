using Newtonsoft.Json;

namespace Models.Application;

public class VonageUrl
{
    public string Address { get; set; }
    
    [JsonProperty("http_method")]
    public string HttpMethod { get; set; }
}