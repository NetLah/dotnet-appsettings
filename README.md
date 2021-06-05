# dotnet-appsettings - .NET tools

A tool to convert appsettings.json files to json name-value format support bulk update to Application Settings on Azure AppService.

- appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "Array": [
    {
      "Name": "Value1"
    },
    {
      "Name": "Value2"
    }
  ],
  "AllowedHosts": "*"
}
```

- Azure AppService / Configuration / Application Settings / Advanced edit (https://docs.microsoft.com/en-us/azure/app-service/configure-common#edit-in-bulk)

![Edit in bulk](docs/bulk-edit-app-settings.png)

```json
[
  {
    "name": "AllowedHosts",
    "value": "*",
    "slotSetting": false
  },
  {
    "name": "Array__0__Name",
    "value": "Value1",
    "slotSetting": false
  },
  {
    "name": "Array__1__Name",
    "value": "Value2",
    "slotSetting": false
  },
  {
    "name": "Logging__LogLevel__Default",
    "value": "Information",
    "slotSetting": false
  },
  {
    "name": "Logging__LogLevel__Microsoft",
    "value": "Warning",
    "slotSetting": false
  },
  {
    "name": "Logging__LogLevel__Microsoft.Hosting.Lifetime",
    "value": "Information",
    "slotSetting": false
  }
]
```

## Getting started

### Installation dotnet tool globally

Download and install the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet/3.1) or [.NET Core 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) or newer. Once installed the .NET Core SDK, run the following command:

```
dotnet tool install --global dotnet-appsettings
```

If you already have a previous version of dotnet-appsettings installed, you can upgrade to the latest version using the following command:

```
dotnet tool update --global dotnet-appsettings
```

Usage

```
appsettings appsettings.json appsettings.Production.json
```

### Installation dotnet tool to a path

You can install the tool `dotnet-appsettings` to a folder

```
dotnet tool install dotnet-appsettings --tool-path C:\Development\Project1\tools
```

Usage

```
C:\Development\Project1\tools\appsettings.exe appsettings.json appsettings.Production.json
```

### Installation dotnet tool locally

You can either install the tool locally in the project folder scope as https://docs.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use

```
cd /d C:\Development\Project1
dotnet new tool-manifest
dotnet tool install dotnet-appsettings
```

Usage

```
C:\Development\Project1\Core> dotnet appsettings appsettings.json appsettings.Production.json
```

### Usage

```
Command line global:
  appsettings [appsettings.json [appsettings.Production.json]]

Command line local:
  dotnet appsettings [appsettings.json [appsettings.Production.json]]

Parameters:
  -p|--path           path to appsettings.json, appsettings.Production.json
  -o|--output-file    path to output-file.json
  -t|--text           output in text format
  -?|-h|--help        help
  --version           version of this tool
```
