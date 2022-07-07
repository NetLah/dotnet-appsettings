using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAppSettings.Formatters
{
    internal class MapEnvironmentOutputFormatter : BaseEnvironmentOutputFormatter, IOutputFormatter
    {
        public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
        {
            var data = settings.ToDictionary(s => s.Name, s => s.Value);
            return SerializeAsync(stream, data);
        }
    }
}
