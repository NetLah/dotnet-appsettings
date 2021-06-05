using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DotnetAppSettings
{
    internal interface IOutputFormatter
    {
        Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings);
    }
}
