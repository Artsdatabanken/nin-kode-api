# NiN3.Core\NiN3.Core.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 4
- **Number of Files**: 60
- **Number of Files with Incidents**: 1
- **Lines of Code**: 1248
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

## Related Projects

**Depended on by (4)** — projects that reference this one:

- [C:\Repo\Artsdatabanken\nin-kode-api\Infrastructure.Mapping\NiN3.Infrastructure.Mapping.csproj](../projects/NiN3.Infrastructure.Mapping.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Console\NiN3.Console.csproj](../projects/NiN3.Console.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Infrastructure\NiN3.Infrastructure.csproj](../projects/NiN3.Infrastructure.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Tests\NiN3.Tests.csproj](../projects/NiN3.Tests.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (4)"]
        P9["<b>📦&nbsp;NiN3.Console.csproj</b><br/><small>net9.0</small>"]
        P11["<b>📦&nbsp;NiN3.Infrastructure.csproj</b><br/><small>net9.0</small>"]
        P12["<b>📦&nbsp;NiN3.Infrastructure.Mapping.csproj</b><br/><small>net9.0</small>"]
        P13["<b>📦&nbsp;NiN3.Tests.csproj</b><br/><small>net9.0</small>"]
        click P9 "../projects/NiN3.Console.md"
        click P11 "../projects/NiN3.Infrastructure.md"
        click P12 "../projects/NiN3.Infrastructure.Mapping.md"
        click P13 "../projects/NiN3.Tests.md"
    end
    subgraph current["NiN3.Core.csproj"]
        MAIN["<b>📦&nbsp;NiN3.Core.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NiN3.Core.md"
    end
    P9 --> MAIN
    P11 --> MAIN
    P12 --> MAIN
    P13 --> MAIN

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1280 |  |
| ***Total APIs Analyzed*** | ***1280*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

