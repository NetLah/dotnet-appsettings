using Xunit;

namespace DotnetAppSettings.Test;

public class AppSettingConversionTest
{
    private string FullAzure(bool slotSetting0 = false, bool slotSetting1 = false)
    {
        return @"[
  {
    ""name"": ""!Key3"",
    ""value"": ""Value \u00263 \u002B 4"",
    ""slotSetting"": false
  },
  {
    ""name"": ""AllowedHosts"",
    ""value"": ""*"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__0__Name"",
    ""value"": ""!Value1"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__1__Name"",
    ""value"": ""@Value2"",
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
    ""slotSetting"": " + slotSetting1.ToString().ToLower() + @"
  },
  {
    ""name"": ""Logging__LogLevel__Microsoft.Hosting.Lifetime"",
    ""value"": ""Information"",
    ""slotSetting"": " + slotSetting0.ToString().ToLower() + @"
  }
]";
    }

    private static Task<string[]> AppSettingsExecuteAsync(string parameters)
    {
        return SimpleExecuteHelper.ExecuteAsync("DotnetAppSettings", parameters);
    }

    [Fact]
    public async Task Convert_Azure_Main()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.json");

        Assert.Equal(@"[
  {
    ""name"": ""!Key3"",
    ""value"": ""Value \u00263 \u002B 4"",
    ""slotSetting"": false
  },
  {
    ""name"": ""AllowedHosts"",
    ""value"": ""*"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__0__Name"",
    ""value"": ""!Value1"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__1__Name"",
    ""value"": ""@Value2"",
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

        Assert.Equal(FullAzure(), string.Join(Environment.NewLine, result));
    }

    [Fact]
    public async Task Convert_Azure_MainDev_SlotSetting()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --slot-setting Files/appsettings.slotSetting");

        Assert.Equal(FullAzure(true, false), string.Join(Environment.NewLine, result));
    }

    [Fact]
    public async Task Convert_Azure_MainDev_SlotSetting2()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --slot-setting Files/appsettings2.slotSetting");

        Assert.Equal(FullAzure(false, true), string.Join(Environment.NewLine, result));
    }

    [Fact]
    public async Task Convert_Azure_MainDev_SlotSetting3()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --slot-setting Files/appsettings3.slotSetting");

        Assert.Equal(FullAzure(true, true), string.Join(Environment.NewLine, result));
    }

    [Fact]
    public async Task Convert_Azure_DevMain()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.Development.json Files/appsettings.json");
        Assert.Equal(@"[
  {
    ""name"": ""!Key3"",
    ""value"": ""Value \u00263 \u002B 4"",
    ""slotSetting"": false
  },
  {
    ""name"": ""AllowedHosts"",
    ""value"": ""*"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__0__Name"",
    ""value"": ""!Value1"",
    ""slotSetting"": false
  },
  {
    ""name"": ""Array__1__Name"",
    ""value"": ""@Value2"",
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
        Assert.Equal(FullAzure(), string.Join(Environment.NewLine, result));
    }

    [Fact]
    public async Task Convert_DockerCompose_Environemnt()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --environment");
        Assert.Equal(@"- '!Key3=Value &3 + 4'
- AllowedHosts=*
- Array__0__Name=!Value1
- Array__1__Name=@Value2
- ConnectionStrings__DefaultConnection=Data Source=localhost;Initial Catalog=database-73628d830a33;Integrated Security=True;MultipleActiveResultSets=true;
- Logging__LogLevel__Default=Debug
- Logging__LogLevel__Microsoft=Warning
- Logging__LogLevel__Microsoft.Hosting.Lifetime=Information
", string.Join(Environment.NewLine, result));
    }

    [Fact]
    public async Task Convert_DockerCompose_MapEnvironemnt()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --map-environment");
        Assert.Equal(@"'!Key3': Value &3 + 4
AllowedHosts: '*'
Array__0__Name: '!Value1'
Array__1__Name: '@Value2'
ConnectionStrings__DefaultConnection: Data Source=localhost;Initial Catalog=database-73628d830a33;Integrated Security=True;MultipleActiveResultSets=true;
Logging__LogLevel__Default: Debug
Logging__LogLevel__Microsoft: Warning
Logging__LogLevel__Microsoft.Hosting.Lifetime: Information
", string.Join(Environment.NewLine, result));
    }

    [Fact]
    public async Task Convert_JsonEnvironemnt()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --json-environment");
        Assert.Equal(@"{
  ""!Key3"": ""Value \u00263 \u002B 4"",
  ""AllowedHosts"": ""*"",
  ""Array__0__Name"": ""!Value1"",
  ""Array__1__Name"": ""@Value2"",
  ""ConnectionStrings__DefaultConnection"": ""Data Source=localhost;Initial Catalog=database-73628d830a33;Integrated Security=True;MultipleActiveResultSets=true;"",
  ""Logging__LogLevel__Default"": ""Debug"",
  ""Logging__LogLevel__Microsoft"": ""Warning"",
  ""Logging__LogLevel__Microsoft.Hosting.Lifetime"": ""Information""
}", string.Join(Environment.NewLine, result));
    }

    [Fact]
    public async Task Convert_Text()
    {
        var result = await AppSettingsExecuteAsync("Files/appsettings.json Files/appsettings.Development.json --text");
        Assert.Equal(@"!Key3
Value &3 + 4

AllowedHosts
*

Array__0__Name
!Value1

Array__1__Name
@Value2

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

    [Fact]
    public async Task Version_Text()
    {
        var versionString =
#if NETCOREAPP3_1
            " .NET:.NETCoreApp,Version=v3.1"
#elif NET5_0
            " .NET:.NETCoreApp,Version=v5.0"
#elif NET6_0
            " .NET:.NETCoreApp,Version=v6.0"
#elif NET7_0
            " .NET:.NETCoreApp,Version=v7.0"
#else
            " .NET:.NETCoreApp,Version=v8.0"
#endif
            ;
        var result = await AppSettingsExecuteAsync("--version");
        Assert.EndsWith(versionString, string.Join(string.Empty, result));
    }
}
