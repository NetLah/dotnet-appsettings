using Microsoft.Extensions.CommandLineUtils;
using NetLah.Diagnostics;

namespace DotnetAppSettings;

internal class CommandBase
{
    public virtual void Configure(CommandLineApplication command)
    {
        Command = command ?? throw new ArgumentNullException(nameof(command));
        VerboseOption = command.Option("-v|--verbose", "Show verbose output.", CommandOptionType.NoValue);

        command.OnExecute(
            async () =>
            {
                await ValidateAsync();

                return await ExecuteAsync();
            });

        command.LongVersionGetter = GetLongVersion;
        command.ShortVersionGetter = GetShortVersion;
    }

    protected CommandLineApplication? Command { get; private set; }

    protected CommandOption? VerboseOption { get; private set; }

    protected bool IsVerbose => VerboseOption?.HasValue() == true;

    protected virtual Task ValidateAsync() => Task.CompletedTask;

    protected virtual Task<int> ExecuteAsync() => SuccessAsync();

#pragma warning disable CA1822 // Mark members as static
    protected Task<int> SuccessAsync() => Task.FromResult(0);
#pragma warning restore CA1822 // Mark members as static    

    protected static string GetLongVersion()
        => $"v{Assembly.InformationalVersion} Build:{Assembly.BuildTimestampLocal} .NET:{Assembly.FrameworkName}";

    protected static string GetShortVersion()
        => $"v{Assembly.InformationalVersion.Split('+')[0]} Build:{Assembly.BuildTimestampLocal} .NET:{Assembly.FrameworkName}";

    private static IAssemblyInfo? _assembly;

    private static IAssemblyInfo Assembly => _assembly ??= new AssemblyInfo(typeof(CommandBase).Assembly);
}
