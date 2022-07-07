namespace DotnetAppSettings;

internal interface IOutputFormatter
{
    Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings);
}
