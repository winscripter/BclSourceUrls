using System.Text.Json.Serialization;

namespace BclSourceFetcher;

internal class BclObject
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
