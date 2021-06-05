using System.Text.Json.Serialization;

namespace DotnetAppSettings
{
    internal class AzureAppSetting
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("slotSetting")]
        public bool SlotSetting { get; set; }
    }
}
