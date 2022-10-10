using DotnetAppSettings;
using NetLah.Extensions.CommandLineUtils;

try
{
    var app = new CommandLineApplication(throwOnUnexpectedArg: true);
    new RootCommand().Configure(app);
    var result = app.Execute(args);
    return result;
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

return 1;
