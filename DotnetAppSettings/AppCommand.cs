using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace DotnetAppSettings
{
    internal class AppCommand : HelpCommandBase
    {
        private CommandArgument _appsettingJsonArgs;
        private CommandOption _path;
        private CommandOption _outputFile;
        private CommandOption _textFormat;

        public override void Configure(CommandLineApplication command)
        {
            command.FullName = "appsettings.exe";

            _appsettingJsonArgs = command.Argument("appsettingsFiles", "appsettings.json appsettings.Production.json", true);
            _path = command.Option("-p|--path", "path to appsettings.json, appsettings.Production.json", CommandOptionType.SingleValue);
            _outputFile = command.Option("-o|--output-file", "path to output-file.json", CommandOptionType.SingleValue);
            _textFormat = command.Option("-t|--text", "output in text format", CommandOptionType.NoValue);

            base.Configure(command);
        }

        protected override async Task<int> ExecuteAsync()
        {
            var appsettingJsons = _appsettingJsonArgs.Values;
            appsettingJsons ??= new List<string>();
            if (appsettingJsons.Count == 0)
            {
                appsettingJsons.Add("appsettings.json");
                WriteVerbose("Add default: appsettings.json");
            }

            var path = Directory.GetCurrentDirectory();
            if (_path.HasValue())
            {
                var pathOption = _path.Value();
                path = Path.Combine(path, pathOption);
                WriteVerbose($"Set path: {pathOption}");
                if (!Directory.Exists(path))
                {
                    Console.Error.WriteLine($"Directory not found: {path}");
                    Command.ShowHelp();
                    return 1;
                }
            }

            var pathAppsettingJsons = appsettingJsons.Select(appsetting => Path.Combine(path, appsetting)).ToList();
            foreach (var target in pathAppsettingJsons)
            {
                if (!File.Exists(target))
                {
                    Console.Error.WriteLine($"File not found: {target}");
                    Command.ShowHelp();
                    return 1;
                }
            }

            Stream output;
            var isOutputFile = _outputFile.HasValue();
            if (isOutputFile)
            {
                var outputFile = _outputFile.Value();
                output = File.Create(outputFile);
                WriteVerbose($"Output to: {outputFile}");
            }
            else
            {
                output = new MemoryStream();
            }

            try
            {
                var formatter = _textFormat.HasValue() ? (IOutputFormatter)new TextOutputFormatter() : new JsonOutputFormatter();
                var converter = new ConfigurationConverter(pathAppsettingJsons);
                await formatter.WriteAsync(output, converter.ConvertSettings());

                if (!isOutputFile)
                {
                    output.Position = 0;
                    using var reader = new StreamReader(output);
                    var content = reader.ReadToEnd();
                    Console.Out.WriteLine(content);
                }

                return await SuccessAsync();
            }
            finally
            {
                await output.DisposeAsync();
            }
        }

        private void WriteVerbose(string message)
        {
            if (IsVerbose)
            {
                Console.WriteLine(message);
            }
        }
    }
}
