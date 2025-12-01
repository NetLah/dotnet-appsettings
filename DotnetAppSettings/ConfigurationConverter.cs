using Microsoft.Extensions.Configuration;

namespace DotnetAppSettings;

internal class ConfigurationConverter(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public ConfigurationConverter(IEnumerable<string> appsettingsJsons) : this(BuildConfig(appsettingsJsons)) { }

    internal static IConfigurationRoot BuildConfig(IEnumerable<string> appsettingsJsons)
    {
        var builder = new ConfigurationBuilder();
        foreach (var item in appsettingsJsons)
        {
            builder.AddJsonFile(item, optional: false, reloadOnChange: false);
        }
        return builder.Build();
    }

    public List<AzureAppSetting> ConvertSettings(bool? defaultSlotSettingValue, HashSet<string>? slotSettings = null)
    {
        var selectKeys = new HashSet<string>();
        foreach (var item in _configuration
            .AsEnumerable(false)
            .Select(x => x.Key)
            .OrderByDescending(x => x.Length))
        {
            if (!selectKeys.Any(x => x.StartsWith($"{item}:"))) // filter sectionKey, support null value
            {
                selectKeys.Add(item);
            }
        }

        var result = _configuration
            .AsEnumerable(false)
            .Where(kv => selectKeys.Contains(kv.Key))
            .OrderBy(kv => kv.Key)
            .Select(kv =>
            {
                var rawKey = kv.Key;
                var key = rawKey.Replace(":", "__");
                var slotSettingValue = slotSettings == null ? new bool?() : (slotSettings.Contains(rawKey) || slotSettings.Contains(key));
                slotSettingValue ??= defaultSlotSettingValue;

                return new AzureAppSetting(key, kv.Value, slotSettingValue);
            })
            .ToList();

        return result;
    }
}
