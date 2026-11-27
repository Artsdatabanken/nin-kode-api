# NiN3.Infrastructure\NiN3.Infrastructure.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 2
- **Dependants**: 3
- **Number of Files**: 53
- **Number of Files with Incidents**: 3
- **Lines of Code**: 3617
- **Estimated LOC to modify**: 3+ (at least 0,1% of the project)

## Related Projects

**Depends on (2)** — projects this one references:

- [C:\Repo\Artsdatabanken\nin-kode-api\Infrastructure.Mapping\NiN3.Infrastructure.Mapping.csproj](../projects/NiN3.Infrastructure.Mapping.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Core\NiN3.Core.csproj](../projects/NiN3.Core.md)

**Depended on by (3)** — projects that reference this one:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Console\NiN3.Console.csproj](../projects/NiN3.Console.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Tests\NiN3.Tests.csproj](../projects/NiN3.Tests.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NinKode.WebApi\NinKode.WebApi.csproj](../projects/NinKode.WebApi.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (3)"]
        P1["<b>📦&nbsp;NinKode.WebApi.csproj</b><br/><small>net9.0</small>"]
        P9["<b>📦&nbsp;NiN3.Console.csproj</b><br/><small>net9.0</small>"]
        P13["<b>📦&nbsp;NiN3.Tests.csproj</b><br/><small>net9.0</small>"]
        click P1 "../projects/NinKode.WebApi.md"
        click P9 "../projects/NiN3.Console.md"
        click P13 "../projects/NiN3.Tests.md"
    end
    subgraph current["NiN3.Infrastructure.csproj"]
        MAIN["<b>📦&nbsp;NiN3.Infrastructure.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NiN3.Infrastructure.md"
    end
    subgraph downstream["Dependencies (2)"]
        P10["<b>📦&nbsp;NiN3.Core.csproj</b><br/><small>net9.0</small>"]
        P12["<b>📦&nbsp;NiN3.Infrastructure.Mapping.csproj</b><br/><small>net9.0</small>"]
        click P10 "../projects/NiN3.Core.md"
        click P12 "../projects/NiN3.Infrastructure.Mapping.md"
    end
    P1 --> MAIN
    P9 --> MAIN
    P13 --> MAIN
    MAIN --> P10
    MAIN --> P12

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 3 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 4869 |  |
| ***Total APIs Analyzed*** | ***4872*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Azure.Identity | 1.13.2 | — | 🔵 Optional | NuGet package is deprecated |
| Microsoft.EntityFrameworkCore | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Tools | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

