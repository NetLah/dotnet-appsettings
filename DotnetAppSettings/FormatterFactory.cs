using DotnetAppSettings.Formatters;

namespace DotnetAppSettings;

internal static class FormatterFactory
{
    internal static IOutputFormatter Create(bool isMap, bool isEnv, bool isJson, bool isText)
    {
        return isMap
            ? new MapEnvironmentOutputFormatter()
            : isEnv
            ? new ArrayEnvironmentOutputFormatter()
            : isJson
            ? new JsonEnvironmentOutputFormatter()
            : isText ?
            new TextOutputFormatter() :
            new AppServiceJsonOutputFormatter();
    }
}
