# dotnet-appsettings - .NET tools

Tool convert appsettings (.json) files to Azure AppService Application Settings json name-value format (support bulk updating) or Docker Compose environment format (yaml).

## Nuget package

[![NuGet](https://img.shields.io/nuget/v/dotnet-appsettings.svg?style=flat-square&label=nuget&colorB=00b200)](https://www.nuget.org/packages/dotnet-appsettings/)

## Build Status

[![Build Status](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2FNetLah%2Fdotnet-appsettings%2Fbadge%3Fref%3Dmain&style=flat)](https://actions-badge.atrox.dev/NetLah/dotnet-appsettings/goto?ref=main)

## Getting started

### .NET 8.0 Support

Package version `1.0.0` supports:
- .NET SDK 9.0
- .NET SDK 8.0
- .NET SDK 6.0

### Samples

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
      "Name": "!Value1"
    },
    {
      "Name": "@Value2"
    }
  ],
  "!Key3": "Value &3 + 4",
  "AllowedHosts": "*"
}
```

- Environment array syntax format for Docker compose file `docker-compose.yml`

```yml
services:
  webapi:
    environment:
      - '!Key3=Value &3 + 4'
      - AllowedHosts=*
      - Array__0__Name=!Value1
      - Array__1__Name=@Value2
      - Logging__LogLevel__Default=Information
      - Logging__LogLevel__Microsoft=Warning
      - Logging__LogLevel__Microsoft.Hosting.Lifetime=Information
```

- Environment map syntax format for Docker compose file `docker-compose.yml`

```yml
services:
  webapi:
    environment:
      '!Key3': Value &3 + 4
      AllowedHosts: '*'
      Array__0__Name: '!Value1'
      Array__1__Name: '@Value2'
      Logging__LogLevel__Default: Information
      Logging__LogLevel__Microsoft: Warning
      Logging__LogLevel__Microsoft.Hosting.Lifetime: Information
```

- Environment json format for `launchSettings.json`

```json
{
  "profiles": {
    "ConsoleApp1": {
      "commandName": "Project",
      "environmentVariables": {
        "!Key3": "Value \u00263 \u002B 4",
        "AllowedHosts": "*",
        "Array__0__Name": "!Value1",
        "Array__1__Name": "@Value2",
        "Logging__LogLevel__Default": "Information",
        "Logging__LogLevel__Microsoft": "Warning",
        "Logging__LogLevel__Microsoft.Hosting.Lifetime": "Information"
      }
    }
  }
}
```

- Azure AppService / Configuration / Application Settings / Advanced edit (https://docs.microsoft.com/en-us/azure/app-service/configure-common#edit-in-bulk)

![Edit in bulk](https://raw.githubusercontent.com/NetLah/dotnet-appsettings/main/docs/bulk-edit-app-settings.png)

```json
[
  {
    "name": "!Key3",
    "value": "Value \u00263 \u002B 4",
    "slotSetting": false
  },
  {
    "name": "AllowedHosts",
    "value": "*",
    "slotSetting": false
  },
  {
    "name": "Array__0__Name",
    "value": "!Value1",
    "slotSetting": false
  },
  {
    "name": "Array__1__Name",
    "value": "@Value2",
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

- Text format for manually update Azure AppService / Configuration / Application Settings

```txt
!Key3
Value &3 + 4

AllowedHosts
*

Array__0__Name
!Value1

Array__1__Name
@Value2

Logging__LogLevel__Default
Information

Logging__LogLevel__Microsoft
Warning

Logging__LogLevel__Microsoft.Hosting.Lifetime
Information
```

### Installation dotnet tool globally

Download and install the [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet). Once installed the .NET SDK, run the following command to install the tool:

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

Command line tool path:
  "C:\Development\Project1\tools\appsettings.exe" [appsettings.json [appsettings.Production.json]]

C:\>appsettings.exe --help

Convert appsettings (.json) to Azure AppService Application Settings v0.2.3 Build:2023-01-27T11:30:11.448+08:00 .NET:.NETCoreApp,Version=v7.0

Usage: appsettings [arguments] [options]

Arguments:
  appsettingsFiles  appsettings.json appsettings.Production.json

Options:
  -p|--path <path>                          path to appsettings.json, appsettings.Production.json
  -o|--output-file <output-file.json>       path to output-file.json
  --slot-setting <appsettings.slotSetting>  specified file contains keys which SlotSetting=true
  -e|--environment                          output in docker compose environment Array syntax
  -m|--map-environment                      output in docker compose environment Map syntax
  -j|--json-environment                     output in environment json
  -t|--text                                 output in text format
  --skip-slot-setting                       skip SlotSetting=false
  --version                                 Show version information
  -?|-h|--help                              Show help information
  -v|--verbose                              Show verbose output.
```
