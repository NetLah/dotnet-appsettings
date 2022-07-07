using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetAppSettings.Formatters
{
    internal class JsonEnvironmentOutputFormatter : IOutputFormatter
    {
        public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
        {
            var data = settings.ToDictionary(s => s.Name, s => s.Value);
            return JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
