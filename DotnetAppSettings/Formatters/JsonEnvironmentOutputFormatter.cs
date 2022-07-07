using System.Text.Json;

namespace DotnetAppSettings.Formatters;

internal class JsonEnvironmentOutputFormatter : IOutputFormatter
{
    public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
    {
        var data = settings.ToDictionary(s => s.Name, s => s.Value);
        return JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions { WriteIndented = true });
    }
}
