namespace DotnetAppSettings.Formatters;

internal class ArrayEnvironmentOutputFormatter : BaseEnvironmentOutputFormatter, IOutputFormatter
{
    public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
    {
        return SerializeAsync(stream, settings.Select(s => $"{s.Name}={s.Value}"));
    }
}
