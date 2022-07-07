using System;
using System.Threading.Tasks;
using Xunit;

namespace DotnetAppSettings.Test
{
    public class AppSettingConversionTest
    {
        private const string FullAzure = @"[
  {
    ""name"": ""AllowedHosts"",
    ""value"": ""*"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__0__Name"",
    ""value"": ""Value1"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__1__Name"",
    ""value"": ""Value 2"",
    ""slotSetting"": false
  },
  {
    ""name"": ""ConnectionStrings__DefaultConnection"",
    ""value"": ""Data Source=localhost;Initial Catalog=database-73628d830a33;Integrated Security=True;MultipleActiveResultSets=true;"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Default"",
    ""value"": ""Debug"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Microsoft"",
    ""value"": ""Warning"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Microsoft.Hosting.Lifetime"",
    ""value"": ""Information"",
    ""slotSetting"": false
  }
]";

        private static Task<string[]> AppSettingsExecuteAsync(string parameters) => SimpleExecuteHelper.ExecuteAsync("DotnetAppSettings", parameters);

        [Fact]
        public async Task Convert_Azure_Main()
        {
            var result = await AppSettingsExecuteAsync("Files/appsettings.json");

            Assert.Equal(@"[
  {
    ""name"": ""AllowedHosts"",
    ""value"": ""*"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__0__Name"",
    ""value"": ""Value1"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__1__Name"",
    ""value"": ""Value 2"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Default"",
    ""value"": ""Information"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Microsoft"",
    ""value"": ""Warning"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Microsoft.Hosting.Lifetime"",
    ""value"": ""Information"",
    ""slotSetting"": false
  }
]", string.Join(Environment.NewLine, result));
        }

        [Fact]
        public async Task Convert_Azure_MainDev()
        {
            var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json");

            Assert.Equal(FullAzure, string.Join(Environment.NewLine, result));
        }

        [Fact]
        public async Task Convert_Azure_DevMain()
        {
            var result = await AppSettingsExecuteAsync("Files/appsettings.Development.json Files/appsettings.json");
            Assert.Equal(@"[
  {
    ""name"": ""AllowedHosts"",
    ""value"": ""*"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__0__Name"",
    ""value"": ""Value1"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__1__Name"",
    ""value"": ""Value 2"",
    ""slotSetting"": false
  },
  {
    ""name"": ""ConnectionStrings__DefaultConnection"",
    ""value"": ""Data Source=localhost;Initial Catalog=database-73628d830a33;Integrated Security=True;MultipleActiveResultSets=true;"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Default"",
    ""value"": ""Information"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Microsoft"",
    ""value"": ""Warning"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Logging__LogLevel__Microsoft.Hosting.Lifetime"",
    ""value"": ""Information"",
    ""slotSetting"": false
  }
]", string.Join(Environment.NewLine, result));
        }

        [Fact]
        public async Task Convert_Azure_Path()
        {
            var result = await AppSettingsExecuteAsync("--path Files appsettings.json appsettings.Development.json");
            Assert.Equal(FullAzure, string.Join(Environment.NewLine, result));
        }

        [Fact]
        public async Task Convert_DockerCompose_Environemnt()
        {
            var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --environment");
            Assert.Equal(@"- AllowedHosts=*
- Array__0__Name=Value1
- Array__1__Name=Value 2
- ConnectionStrings__DefaultConnection=Data Source=localhost;Initial Catalog=database-73628d830a33;Integrated Security=True;MultipleActiveResultSets=true;
- Logging__LogLevel__Default=Debug
- Logging__LogLevel__Microsoft=Warning
- Logging__LogLevel__Microsoft.Hosting.Lifetime=Information
", string.Join(Environment.NewLine, result));
        }

        [Fact]
        public async Task Convert_Text()
        {
            var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --text");
            Assert.Equal(@"AllowedHosts
*

Array__0__Name
Value1

Array__1__Name
Value 2

ConnectionStrings__DefaultConnection
Data Source=localhost;Initial Catalog=database-73628d830a33;Integrated Security=True;MultipleActiveResultSets=true;

Logging__LogLevel__Default
Debug

Logging__LogLevel__Microsoft
Warning

Logging__LogLevel__Microsoft.Hosting.Lifetime
Information
", string.Join(Environment.NewLine, result));
        }
    }
}
