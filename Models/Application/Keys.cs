using Newtonsoft.Json;

namespace Models.Application;

public class Keys
{
    [JsonProperty("public_key")]
    public string PublicKey { get; set; }
    
    [JsonProperty("private_key")]
    public string PrivateKey { get; set; }
}