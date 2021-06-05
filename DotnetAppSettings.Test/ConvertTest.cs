using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DotnetAppSettings.Test
{
    public class ConvertTest
    {
        private readonly List<AzureAppSetting> settings = new()
        {
            new AzureAppSetting { Name = "Key1", Value = "Value1" },
            new AzureAppSetting { Name = "Key2__Sub2", Value = "2021" },
            new AzureAppSetting { Name = "Parrent__Array6__0__Name", Value = "Element1" },
            new AzureAppSetting { Name = "Parrent__Array6__1__Additional", Value = "Add2" },
            new AzureAppSetting { Name = "Parrent__Array6__1__Name", Value = "Element2" },
            new AzureAppSetting { Name = "Parrent__Child1__Key3", Value = "Value3" },
            new AzureAppSetting { Name = "Parrent__Child1__Key4", Value = "" },
            new AzureAppSetting { Name = "Parrent__Child1__Key5", Value = "" },
        };

        private IConfigurationRoot GetAppsettingsConfiguration(string resourceName = "DotnetAppSettings.Test.appsettings.json")
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

            var result = service.ConvertSettings();

            Assert.Equal(result, settings, new AzureAppSettingComparer());
        }

        private class AzureAppSettingComparer : IEqualityComparer<AzureAppSetting>
        {
            public bool Equals([AllowNull] AzureAppSetting x, [AllowNull] AzureAppSetting y)
                => x.Name == y.Name &&
                    x.SlotSetting == y.SlotSetting &&
                    x.Value == y.Value;

            public int GetHashCode([DisallowNull] AzureAppSetting obj)
                => (obj.Name?.GetHashCode() ?? 0) ^
                obj.SlotSetting.GetHashCode() ^
                (obj.Value?.GetHashCode() ?? 0);
        }
    }
}
