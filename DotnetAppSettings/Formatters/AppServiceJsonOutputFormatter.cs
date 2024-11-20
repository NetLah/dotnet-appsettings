using System.Text.Json;

namespace DotnetAppSettings.Formatters;

internal class AppServiceJsonOutputFormatter : IOutputFormatter
{
    public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
    {
        return JsonSerializer.SerializeAsync(stream, settings, new JsonSerializerOptions { WriteIndented = true });
    }
}
