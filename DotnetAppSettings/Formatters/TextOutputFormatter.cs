﻿namespace DotnetAppSettings.Formatters;

internal class TextOutputFormatter : IOutputFormatter
{
    public async Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
    {
        var content = settings
            .SelectMany(s => new[] {
                string.Empty,
                s.Name,
                s.Value,
                // $"SlotSetting={s.SlotSetting }"  Text format not include SlotSetting
            })
            .Skip(1);

        using var writer = new StreamWriter(stream, leaveOpen: true);

        foreach (var line in content)
        {
            await writer.WriteLineAsync(line);
        }
    }
}
