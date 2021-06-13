using DotnetAppSettings.Formatters;

namespace DotnetAppSettings
{
    internal static class FormatterFactory
    {
        internal static IOutputFormatter Create(bool isEnv, bool isText)
        {
            if (isEnv)
                return new EnvironmentOutputFormatter();

            return isText ?
                 (IOutputFormatter)new TextOutputFormatter() :
                new JsonOutputFormatter();
        }
    }
}
