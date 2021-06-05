using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace DotnetAppSettings
{
    internal class CommandBase
    {
        public virtual void Configure(CommandLineApplication command)
        {
            VerboseOption = command.Option("-v|--verbose", "Show verbose output.", CommandOptionType.NoValue);

            command.VersionOption("--version", GetVersion);

            command.OnExecute(
                async () =>
                {
                    await ValidateAsync();

                    return await ExecuteAsync();
                });

            Command = command;
        }

        protected CommandLineApplication Command { get; private set; }

        protected CommandOption VerboseOption { get; private set; }

        protected bool IsVerbose => VerboseOption.HasValue();

        protected virtual Task ValidateAsync() => Task.CompletedTask;

        protected virtual Task<int> ExecuteAsync() => SuccessAsync();

#pragma warning disable CA1822 // Mark members as static
        protected Task<int> SuccessAsync() => Task.FromResult(0);
#pragma warning restore CA1822 // Mark members as static    

        private static string GetVersion()
            => typeof(AppCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
    }
}
