using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace DotnetAppSettings.Test;

public class ConvertTest
{
    private static List<AzureAppSetting> GetSettings(bool? slotSetting)
        => new()
        {
            new AzureAppSetting("Key1", "Value1", slotSetting),
            new AzureAppSetting("Key2__Sub2", "2021", slotSetting),
            new AzureAppSetting("Parrent__Array6__0__Name", "Element1", slotSetting),
            new AzureAppSetting("Parrent__Array6__1__Additional", "Add2", slotSetting),
            new AzureAppSetting("Parrent__Array6__1__Name", "Element2", slotSetting),
            new AzureAppSetting("Parrent__Child1__Key3", "Value3", slotSetting),
            new AzureAppSetting("Parrent__Child1__Key4", "", slotSetting),
            new AzureAppSetting("Parrent__Child1__Key5", "", slotSetting),
        };

    private static IConfigurationRoot GetAppsettingsConfiguration(string resourceName = "DotnetAppSettings.Test.appsettings.json")
    {
        var builder = new ConfigurationBuilder();

        var resource = typeof(ConvertTest)
            .Assembly
            .GetManifestResourceStream(resourceName);

        if (resource != null)
            builder.AddJsonStream(resource);

        return builder.Build();
    }

    [Fact]
    public void ConfigurationConvertTest()
    {
        var service = new ConfigurationConverter(GetAppsettingsConfiguration());

        var result = service.ConvertSettings(false);

        Assert.Equal(result, GetSettings(false), new AzureAppSettingComparer());
    }

    [Fact]
    public void ConfigurationConvert_SkipSlotSettingTest()
    {
        var service = new ConfigurationConverter(GetAppsettingsConfiguration());

        var result = service.ConvertSettings(null);

        Assert.Equal(result, GetSettings(null), new AzureAppSettingComparer());
    }

    private class AzureAppSettingComparer : IEqualityComparer<AzureAppSetting>
    {
        public bool Equals([AllowNull] AzureAppSetting? x, [AllowNull] AzureAppSetting? y)
            => x == null && y == null ||
                (x != null && y != null &&
                x.Name == y.Name &&
                x.SlotSetting == y.SlotSetting &&
                x.Value == y.Value);

        public int GetHashCode([DisallowNull] AzureAppSetting obj)
            => (obj.Name?.GetHashCode() ?? 0) ^
            (obj.SlotSetting?.GetHashCode() ?? 0) ^
            (obj.Value?.GetHashCode() ?? 0);
    }
}
