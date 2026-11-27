# NinKode.Common\NinKode.Common.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 2
- **Number of Files**: 24
- **Number of Files with Incidents**: 2
- **Lines of Code**: 335
- **Estimated LOC to modify**: 3+ (at least 0,9% of the project)

## Related Projects

**Depends on (1)** — projects this one references:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.Database\NiN.Database.csproj](../projects/NiN.Database.md)

**Depended on by (2)** — projects that reference this one:

- [C:\Repo\Artsdatabanken\nin-kode-api\NinKode.Database\NinKode.Database.csproj](../projects/NinKode.Database.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NinKode.WebApi\NinKode.WebApi.csproj](../projects/NinKode.WebApi.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>📦&nbsp;NinKode.WebApi.csproj</b><br/><small>net9.0</small>"]
        P3["<b>📦&nbsp;NinKode.Database.csproj</b><br/><small>net9.0</small>"]
        click P1 "../projects/NinKode.WebApi.md"
        click P3 "../projects/NinKode.Database.md"
    end
    subgraph current["NinKode.Common.csproj"]
        MAIN["<b>📦&nbsp;NinKode.Common.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NinKode.Common.md"
    end
    subgraph downstream["Dependencies (1)"]
        P4["<b>📦&nbsp;NiN.Database.csproj</b><br/><small>net9.0</small>"]
        click P4 "../projects/NiN.Database.md"
    end
    P1 --> MAIN
    P3 --> MAIN
    MAIN --> P4

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 3 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 273 |  |
| ***Total APIs Analyzed*** | ***276*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |
| System.Text.Json | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

