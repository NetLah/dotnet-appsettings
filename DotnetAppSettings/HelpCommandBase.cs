using Microsoft.Extensions.CommandLineUtils;

namespace DotnetAppSettings
{
    internal class HelpCommandBase : CommandBase
    {
        public override void Configure(CommandLineApplication command)
        {
            command.HelpOption("-?|-h|--help");

            base.Configure(command);
        }

        /// Use in case show help by default
        ///protected override Task<int> ExecuteAsync()
        ///{
        ///    Command.ShowHelp();
        ///    return base.ExecuteAsync();
        ///}
    }
}
