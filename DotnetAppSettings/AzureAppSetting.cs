using System.Text.Json.Serialization;

namespace DotnetAppSettings;

internal class AzureAppSetting
{
    public AzureAppSetting(string name, string? value, bool? slotSetting)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value;
        SlotSetting = slotSetting;
    }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("value")]
    public string? Value { get; }

    [JsonPropertyName("slotSetting")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SlotSetting { get; }
}
