using YamlDotNet.Serialization;

namespace DotnetAppSettings.Formatters;

internal abstract class BaseEnvironmentOutputFormatter
{
    public static Task SerializeAsync<TData>(Stream stream, TData data)
    {
        var serializer = new SerializerBuilder().Build();
        using var writer = new StreamWriter(stream, leaveOpen: true);
        serializer.Serialize(writer, data);
        return Task.CompletedTask;
    }
}
