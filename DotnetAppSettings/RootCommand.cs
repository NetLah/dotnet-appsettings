using NetLah.Extensions.CommandLineUtils;
using System.Text;

namespace DotnetAppSettings;

internal class RootCommand : HelpCommandBase
{
    private CommandArgument? _appsettingJsonArgs;
    private CommandOption? _path;
    private CommandOption? _outputFile;
    private CommandOption? _slotSetting;
    private CommandOption? _arrayEnvironmentFormat;
    private CommandOption? _mapEnvironmentFormat;
    private CommandOption? _jsonEnvironmentFormat;
    private CommandOption? _textFormat;
    private CommandOption? _skipSlotSetting;

    public override void Configure(CommandLineApplication command)
    {
        command.Name = "appsettings";
        command.FullName = "Convert appsettings (.json) to Azure AppService Application Settings";

        _appsettingJsonArgs = command.Argument("appsettingsFiles", "appsettings.json appsettings.Production.json", true);
        _path = command.Option("-p|--path <path>", "path to appsettings.json, appsettings.Production.json", CommandOptionType.SingleValue);
        _outputFile = command.Option("-o|--output-file <output-file.json>", "path to output-file.json", CommandOptionType.SingleValue);
        _slotSetting = command.Option("--slot-setting <appsettings.slotSetting>", "specified file contains keys which SlotSetting=true", CommandOptionType.SingleValue);
        _arrayEnvironmentFormat = command.Option("-e|--environment", "output in docker compose environment Array syntax", CommandOptionType.NoValue);
        _mapEnvironmentFormat = command.Option("-m|--map-environment", "output in docker compose environment Map syntax", CommandOptionType.NoValue);
        _jsonEnvironmentFormat = command.Option("-j|--json-environment", "output in environment json", CommandOptionType.NoValue);
        _textFormat = command.Option("-t|--text", "output in text format", CommandOptionType.NoValue);
        _skipSlotSetting = command.Option("--skip-slot-setting", "skip SlotSetting=false", CommandOptionType.NoValue);

        command.VersionOption("--version", GetShortVersion, GetLongVersion);

        base.Configure(command);
    }

    protected override async Task<int> ExecuteAsync()
    {
        const string nullError = "Call Configure() method first";
        if (_appsettingJsonArgs == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (_path == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (_outputFile == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (_slotSetting == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (_arrayEnvironmentFormat == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (_mapEnvironmentFormat == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (_jsonEnvironmentFormat == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (_textFormat == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (_skipSlotSetting == null)
        {
            throw new NullReferenceException(nullError);
        }

        if (Command == null)
        {
            throw new NullReferenceException(nullError);
        }

        var appsettingJsons = _appsettingJsonArgs.Values;
        appsettingJsons ??= [];
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
            var fullpath = Path.GetFullPath(path);
            if (!Directory.Exists(fullpath))
            {
                Console.Error.WriteLine($"Directory not found: {path}");
                Command.ShowHelp();
                return 1;
            }
            path = fullpath;
        }

        var pathAppsettingJsons = appsettingJsons.Select(appsetting => Path.GetFullPath(Path.Combine(path, appsetting))).ToList();
        foreach (var target in pathAppsettingJsons)
        {
            if (!File.Exists(target))
            {
                Console.Error.WriteLine($"File not found: {target}");
                Command.ShowHelp();
                return 1;
            }
        }

        HashSet<string>? slotSettings = null;
        var slotSettingFile = _slotSetting.Value();
        if (slotSettingFile != null)
        {
            var slotSettingFullPath = Path.GetFullPath(Path.Combine(path, slotSettingFile));
            if (!File.Exists(slotSettingFullPath))
            {
                Console.Error.WriteLine($"File not found: {slotSettingFullPath}");
                Command.ShowHelp();
                return 1;
            }

            slotSettings = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using var reader = new StreamReader(slotSettingFullPath);
            string? line = null;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (!string.IsNullOrEmpty(line) && !line.StartsWith('#') && !line.StartsWith(';'))
                {
                    slotSettings.Add(line);
                }
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
            var formatter = FormatterFactory.Create(_mapEnvironmentFormat.HasValue(), _arrayEnvironmentFormat.HasValue(), _jsonEnvironmentFormat.HasValue(), _textFormat.HasValue());
            var converter = new ConfigurationConverter(pathAppsettingJsons);
            var defaultSlotSettingValue = _skipSlotSetting.HasValue() ? new bool?() : false;
            await formatter.WriteAsync(output, converter.ConvertSettings(defaultSlotSettingValue, slotSettings));

            if (isOutputFile)
            {
                await output.WriteAsync(Encoding.ASCII.GetBytes(Environment.NewLine));
            }
            else
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
