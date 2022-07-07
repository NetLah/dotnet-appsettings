namespace DotnetAppSettings.Formatters;

internal class MapEnvironmentOutputFormatter : BaseEnvironmentOutputFormatter, IOutputFormatter
{
    public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
    {
        var data = settings.ToDictionary(s => s.Name, s => s.Value);
        return SerializeAsync(stream, data);
    }
}
