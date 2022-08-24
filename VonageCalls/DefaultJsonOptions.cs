using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VonageCalls;

internal sealed class DefaultJsonOptions
{
    public static JsonSerializerSettings DefaJsonSerializerSettings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        },
        Formatting = Formatting.Indented
    };
}