# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v11.0.

Detailed findings live alongside this file in `assessment/`. This page is the index: read it first, then open only the documents you need.

## Table of Contents

- [Executive Summary](#executive-summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Detailed Reports](#detailed-reports)
  - [Projects Relationship Graph](assessment/project-graph.md)
  - [Aggregate NuGet packages details](assessment/nuget/aggregate-packages.md)
  - [Most Frequent API Issues (complete list)](assessment/api-issues/most-frequent-api-issues.md)
  - [Project Details](#project-details)

## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 13 | All require upgrade |
| Total NuGet Packages | 246 | 23 need upgrade |
| Total Code Files | 270 |  |
| Total Code Files with Incidents | 30 |  |
| Total Lines of Code | 19623 |  |
| Total Number of Issues | 140 |  |
| Proposed Target Framework | net11.0 |  |
| Estimated LOC to modify | 56+ | at least 0,3% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Binding Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
| [Infrastructure.Mapping\NiN3.Infrastructure.Mapping.csproj](assessment/projects/NiN3.Infrastructure.Mapping.md) | net9.0 | 🟢 Low | 4 | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [NiN.CmsImpExp\NiN.CmsImpExp.csproj](assessment/projects/NiN.CmsImpExp.md) | net9.0 | 🟢 Low | 3 | 6 | 0 | 6+ | DotNetCoreApp, Sdk Style = True |
| [NiN.Console\NiN.Console.csproj](assessment/projects/NiN.Console.md) | net9.0 | 🟢 Low | 6 | 1 | 0 | 1+ | DotNetCoreApp, Sdk Style = True |
| [NiN.Database\NiN.Database.csproj](assessment/projects/NiN.Database.md) | net9.0 | 🟢 Low | 7 | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [NiN.ExportImport\NiN.ExportImport.csproj](assessment/projects/NiN.ExportImport.md) | net9.0 | 🟢 Low | 4 | 2 | 0 | 2+ | ClassLibrary, Sdk Style = True |
| [NiN.Test_CmsImpExp\NiN.Test_CmsImpExp.csproj](assessment/projects/NiN.Test_CmsImpExp.md) | net9.0 | 🟢 Low | 4 | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |
| [NiN3.Console\NiN3.Console.csproj](assessment/projects/NiN3.Console.md) | net9.0 | 🟢 Low | 8 | 6 | 0 | 6+ | DotNetCoreApp, Sdk Style = True |
| [NiN3.Core\NiN3.Core.csproj](assessment/projects/NiN3.Core.md) | net9.0 | 🟢 Low | 3 | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [NiN3.Infrastructure\NiN3.Infrastructure.csproj](assessment/projects/NiN3.Infrastructure.md) | net9.0 | 🟢 Low | 7 | 3 | 0 | 3+ | ClassLibrary, Sdk Style = True |
| [NiN3.Tests\NiN3.Tests.csproj](assessment/projects/NiN3.Tests.md) | net9.0 | 🟢 Low | 7 | 1 | 0 | 1+ | DotNetCoreApp, Sdk Style = True |
| [NinKode.Common\NinKode.Common.csproj](assessment/projects/NinKode.Common.md) | net9.0 | 🟢 Low | 4 | 3 | 0 | 3+ | ClassLibrary, Sdk Style = True |
| [NinKode.Database\NinKode.Database.csproj](assessment/projects/NinKode.Database.md) | net9.0 | 🟢 Low | 8 | 1 | 0 | 1+ | DotNetCoreApp, Sdk Style = True |
| [NinKode.WebApi\NinKode.WebApi.csproj](assessment/projects/NinKode.WebApi.md) | net9.0 | 🟢 Low | 6 | 33 | 0 | 33+ | AspNetCore, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 223 | 90,7% |
| ⚠️ Incompatible | 5 | 2,0% |
| 🔄 Upgrade Recommended | 18 | 7,3% |
| ***Total NuGet Packages*** | ***246*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 4 | High - Require code changes |
| 🟡 Source Incompatible | 3 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 49 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 22039 |  |
| ***Total APIs Analyzed*** | ***22095*** |  |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| T:Microsoft.AspNetCore.Mvc.ProducesResponseTypeAttribute | 32 | 57,1% | Behavioral Change |
| T:System.Text.Json.JsonDocument | 5 | 8,9% | Behavioral Change |
| M:Microsoft.Extensions.Configuration.ConfigurationBinder.GetValue''1(Microsoft.Extensions.Configuration.IConfiguration,System.String) | 4 | 7,1% | Binary Incompatible |
| T:System.Uri | 4 | 7,1% | Behavioral Change |
| M:System.IO.MemoryStream.#ctor | 3 | 5,4% | Behavioral Change |
| M:Microsoft.Extensions.Configuration.EnvironmentVariablesExtensions.AddEnvironmentVariables(Microsoft.Extensions.Configuration.IConfigurationBuilder) | 2 | 3,6% | Behavioral Change |
| M:System.IO.Path.Combine(System.ReadOnlySpan{System.String}) | 2 | 3,6% | Source Incompatible |
| M:Microsoft.Extensions.Logging.ConsoleLoggerExtensions.AddConsole(Microsoft.Extensions.Logging.ILoggingBuilder) | 1 | 1,8% | Behavioral Change |
| M:System.Net.WebClient.#ctor | 1 | 1,8% | Source Incompatible |
| M:System.Uri.#ctor(System.String) | 1 | 1,8% | Behavioral Change |

The table above is the top 10. See [the complete list](assessment/api-issues/most-frequent-api-issues.md) for every affected API.

## Detailed Reports

- [Projects Relationship Graph](assessment/project-graph.md)
- [Aggregate NuGet packages details](assessment/nuget/aggregate-packages.md)
- [Most Frequent API Issues (complete list)](assessment/api-issues/most-frequent-api-issues.md)

### Project Details

- [Infrastructure.Mapping\NiN3.Infrastructure.Mapping.csproj](assessment/projects/NiN3.Infrastructure.Mapping.md)
- [NiN.CmsImpExp\NiN.CmsImpExp.csproj](assessment/projects/NiN.CmsImpExp.md)
- [NiN.Console\NiN.Console.csproj](assessment/projects/NiN.Console.md)
- [NiN.Database\NiN.Database.csproj](assessment/projects/NiN.Database.md)
- [NiN.ExportImport\NiN.ExportImport.csproj](assessment/projects/NiN.ExportImport.md)
- [NiN.Test_CmsImpExp\NiN.Test_CmsImpExp.csproj](assessment/projects/NiN.Test_CmsImpExp.md)
- [NiN3.Console\NiN3.Console.csproj](assessment/projects/NiN3.Console.md)
- [NiN3.Core\NiN3.Core.csproj](assessment/projects/NiN3.Core.md)
- [NiN3.Infrastructure\NiN3.Infrastructure.csproj](assessment/projects/NiN3.Infrastructure.md)
- [NiN3.Tests\NiN3.Tests.csproj](assessment/projects/NiN3.Tests.md)
- [NinKode.Common\NinKode.Common.csproj](assessment/projects/NinKode.Common.md)
- [NinKode.Database\NinKode.Database.csproj](assessment/projects/NinKode.Database.md)
- [NinKode.WebApi\NinKode.WebApi.csproj](assessment/projects/NinKode.WebApi.md)


