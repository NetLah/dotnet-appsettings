using System;
using Microsoft.Extensions.CommandLineUtils;

namespace DotnetAppSettings
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var app = new CommandLineApplication(throwOnUnexpectedArg: true);
                new AppCommand().Configure(app);
                var result = app.Execute(args);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return 1;
        }
    }
}
