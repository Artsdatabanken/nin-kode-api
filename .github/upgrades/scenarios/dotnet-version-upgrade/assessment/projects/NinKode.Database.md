# NinKode.Database\NinKode.Database.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 3
- **Dependants**: 2
- **Number of Files**: 51
- **Number of Files with Incidents**: 2
- **Lines of Code**: 6087
- **Estimated LOC to modify**: 1+ (at least 0,0% of the project)

## Related Projects

**Depends on (3)** — projects this one references:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.Database\NiN.Database.csproj](../projects/NiN.Database.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.ExportImport\NiN.ExportImport.csproj](../projects/NiN.ExportImport.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NinKode.Common\NinKode.Common.csproj](../projects/NinKode.Common.md)

**Depended on by (2)** — projects that reference this one:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.Console\NiN.Console.csproj](../projects/NiN.Console.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NinKode.WebApi\NinKode.WebApi.csproj](../projects/NinKode.WebApi.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>📦&nbsp;NinKode.WebApi.csproj</b><br/><small>net9.0</small>"]
        P5["<b>📦&nbsp;NiN.Console.csproj</b><br/><small>net9.0</small>"]
        click P1 "../projects/NinKode.WebApi.md"
        click P5 "../projects/NiN.Console.md"
    end
    subgraph current["NinKode.Database.csproj"]
        MAIN["<b>📦&nbsp;NinKode.Database.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NinKode.Database.md"
    end
    subgraph downstream["Dependencies (3)"]
        P2["<b>📦&nbsp;NinKode.Common.csproj</b><br/><small>net9.0</small>"]
        P4["<b>📦&nbsp;NiN.Database.csproj</b><br/><small>net9.0</small>"]
        P6["<b>📦&nbsp;NiN.ExportImport.csproj</b><br/><small>net9.0</small>"]
        click P2 "../projects/NinKode.Common.md"
        click P4 "../projects/NiN.Database.md"
        click P6 "../projects/NiN.ExportImport.md"
    end
    P1 --> MAIN
    P5 --> MAIN
    MAIN --> P2
    MAIN --> P4
    MAIN --> P6

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 1 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 5549 |  |
| ***Total APIs Analyzed*** | ***5550*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Design | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.Extensions.Configuration.Binder | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.Extensions.Configuration.EnvironmentVariables | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |
| RavenDB.Database | 3.5.9 | — | 🔴 Mandatory | NuGet package is incompatible |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

