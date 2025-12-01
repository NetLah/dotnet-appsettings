using DotnetAppSettings.Formatters;
using Xunit;

namespace DotnetAppSettings.Test.Formatters;

public class FormattersTest
{
    private readonly List<AzureAppSetting> settings =
    [
        new AzureAppSetting("KEY", "VALUE1", false),
        new AzureAppSetting("key__subkey", "Value 2", true),
        new AzureAppSetting("array3__0", "Value3", null),
    ];

    private static string ReadContent(MemoryStream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    [Fact]
    public async Task AppServiceJsonOutputFormatterTest()
    {
        var service = new AppServiceJsonOutputFormatter();
        using var stream = new MemoryStream();

        await service.WriteAsync(stream, settings);

        var context = ReadContent(stream);

        Assert.Equal(@"[
  {
    ""name"": ""KEY"",
    ""value"": ""VALUE1"",
    ""slotSetting"": false
  },
  {
    ""name"": ""key__subkey"",
    ""value"": ""Value 2"",
    ""slotSetting"": true
  },
  {
    ""name"": ""array3__0"",
    ""value"": ""Value3""
  }
]", context);
    }

    [Fact]
    public async Task TextOutputFormatterTest()
    {
        var service = new TextOutputFormatter();
        using var stream = new MemoryStream();

        await service.WriteAsync(stream, settings);

        var context = ReadContent(stream);

        Assert.Equal(@"KEY
VALUE1

key__subkey
Value 2

array3__0
Value3
", context);
    }

    [Fact]
    public async Task ArrayEnvironmentOutputFormatterTest()
    {
        var service = new ArrayEnvironmentOutputFormatter();
        using var stream = new MemoryStream();

        await service.WriteAsync(stream, settings);

        var context = ReadContent(stream);

        Assert.Equal(@"- KEY=VALUE1
- key__subkey=Value 2
- array3__0=Value3
", context);
    }

    [Fact]
    public async Task MapEnvironmentOutputFormatterTest()
    {
        var service = new MapEnvironmentOutputFormatter();
        using var stream = new MemoryStream();

        await service.WriteAsync(stream, settings);

        var context = ReadContent(stream);

        Assert.Equal(@"KEY: VALUE1
key__subkey: Value 2
array3__0: Value3
", context);
    }

    [Fact]
    public async Task JsonEnvironmentOutputFormatterTest()
    {
        var service = new JsonEnvironmentOutputFormatter();
        using var stream = new MemoryStream();

        await service.WriteAsync(stream, settings);

        var context = ReadContent(stream);

        Assert.Equal(@"{
  ""KEY"": ""VALUE1"",
  ""key__subkey"": ""Value 2"",
  ""array3__0"": ""Value3""
}", context);
    }
}
