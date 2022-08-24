using Newtonsoft.Json;

namespace Models.Application;

public class Privacy
{
    [JsonProperty("improve_ai")]
    public bool ImproveAi { get; set; }
}