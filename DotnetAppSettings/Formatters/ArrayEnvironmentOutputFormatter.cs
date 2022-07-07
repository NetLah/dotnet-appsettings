namespace DotnetAppSettings.Formatters;

internal class ArrayEnvironmentOutputFormatter : BaseEnvironmentOutputFormatter, IOutputFormatter
{
    public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
    {
        var data = settings.Select(s => $"{s.Name}={s.Value}");
        return SerializeAsync(stream, data);
    }
}
