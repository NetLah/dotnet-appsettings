using System.Text.Json.Serialization;

namespace DotnetAppSettings;

internal class AzureAppSetting(string name, string? value, bool? slotSetting)
{
    [JsonPropertyName("name")]
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    [JsonPropertyName("value")]
    public string? Value { get; } = value;

    [JsonPropertyName("slotSetting")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SlotSetting { get; } = slotSetting;
}
