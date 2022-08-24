using System.Text.Json;

namespace VonageCalls.Tests;

internal sealed class DefaultJsonOptions
{
    public static JsonSerializerOptions Settings => new JsonSerializerOptions
        {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
}