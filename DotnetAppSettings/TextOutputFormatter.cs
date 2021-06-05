using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAppSettings
{
    internal class TextOutputFormatter : IOutputFormatter
    {
        public async Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
        {
            var content = settings
                .SelectMany(s => new[] { string.Empty, s.Name, s.Value, $"SlotSetting={s.SlotSetting }" })
                .Skip(1);

            using var writer = new StreamWriter(stream, leaveOpen: true);

            foreach (var line in content)
            {
                await writer.WriteLineAsync(line);
            }
        }
    }
}
