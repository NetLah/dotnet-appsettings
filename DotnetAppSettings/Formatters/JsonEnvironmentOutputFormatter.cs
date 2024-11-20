using System.Text.Json;

namespace DotnetAppSettings.Formatters;

internal class JsonEnvironmentOutputFormatter : IOutputFormatter
{
    public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
    {
        return JsonSerializer.SerializeAsync(stream, settings.ToDictionary(s => s.Name, s => s.Value), new JsonSerializerOptions { WriteIndented = true });
    }
}
