using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace DotnetAppSettings
{
    internal class ConfigurationConverter
    {
        private readonly IConfiguration _configuration;

        public ConfigurationConverter(IEnumerable<string> appsettingsJsons) : this(BuildConfig(appsettingsJsons)) { }

        public ConfigurationConverter(IConfiguration configuration) => _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        internal static IConfigurationRoot BuildConfig(IEnumerable<string> appsettingsJsons)
        {
            var builder = new ConfigurationBuilder();
            foreach (var item in appsettingsJsons)
            {
                builder.AddJsonFile(item, optional: false, reloadOnChange: false);
            }
            return builder.Build();
        }

        public List<AzureAppSetting> ConvertSettings()
        {
            var result = _configuration
                    .AsEnumerable(false)
                    .Where(kv => kv.Value != null)
                    .OrderBy(kv => kv.Key)
                    .Select(kv => new AzureAppSetting { Name = kv.Key.Replace(":", "__"), Value = kv.Value })
                    .ToList();

            return result;
        }
    }
}
