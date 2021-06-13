using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAppSettings.Formatters
{
    internal class EnvironmentOutputFormatter : IOutputFormatter
    {
        public async Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
        {
            var content = settings
                .Select(s => $"- {s.Name}={ s.Value}");

            using var writer = new StreamWriter(stream, leaveOpen: true);

            foreach (var line in content)
            {
                await writer.WriteLineAsync(line);
            }
        }
    }
}
