using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DotnetAppSettings.Test
{
    public class FormattersTest
    {
        private readonly List<AzureAppSetting> settings = new()
        {
            new AzureAppSetting { Name = "KEY", Value = "VALUE1" },
            new AzureAppSetting { Name = "key__subkey", Value = "Value2", SlotSetting = true },
        };

        private static string ReadContent(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        [Fact]
        public async Task JsonOutputFormatterTest()
        {
            var service = new JsonOutputFormatter();
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
    ""value"": ""Value2"",
    ""slotSetting"": true
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
SlotSetting=False

key__subkey
Value2
SlotSetting=True
", context);
        }
    }
}
