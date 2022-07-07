using DotnetAppSettings.Formatters;

namespace DotnetAppSettings
{
    internal static class FormatterFactory
    {
        internal static IOutputFormatter Create(bool isMap, bool isEnv, bool isText)
        {
            if (isMap)
                return new MapEnvironmentOutputFormatter();

            if (isEnv)
                return new ArrayEnvironmentOutputFormatter();

            return isText ?
                 (IOutputFormatter)new TextOutputFormatter() :
                new AppServiceJsonOutputFormatter();
        }
    }
}
