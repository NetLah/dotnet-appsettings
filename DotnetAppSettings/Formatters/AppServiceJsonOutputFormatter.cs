using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetAppSettings.Formatters
{
    internal class AppServiceJsonOutputFormatter : IOutputFormatter
    {
        public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
            => JsonSerializer.SerializeAsync(stream, settings, new JsonSerializerOptions { WriteIndented = true });
    }
}
