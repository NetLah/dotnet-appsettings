using NetLah.Extensions.CommandLineUtils;

namespace DotnetAppSettings;

internal class HelpCommandBase : CommandBase
{
    public override void Configure(CommandLineApplication command)
    {
        command.HelpOption("-?|-h|--help");

        base.Configure(command);
    }
}
