# NiN.Test_CmsImpExp\NiN.Test_CmsImpExp.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 1
- **Dependants**: 1
- **Number of Files**: 6
- **Number of Files with Incidents**: 1
- **Lines of Code**: 308
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

## Related Projects

**Depends on (1)** — projects this one references:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.CmsImpExp\NiN.CmsImpExp.csproj](../projects/NiN.CmsImpExp.md)

**Depended on by (1)** — projects that reference this one:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.Console\NiN.Console.csproj](../projects/NiN.Console.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P5["<b>📦&nbsp;NiN.Console.csproj</b><br/><small>net9.0</small>"]
        click P5 "../projects/NiN.Console.md"
    end
    subgraph current["NiN.Test_CmsImpExp.csproj"]
        MAIN["<b>📦&nbsp;NiN.Test_CmsImpExp.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NiN.Test_CmsImpExp.md"
    end
    subgraph downstream["Dependencies (1)"]
        P7["<b>📦&nbsp;NiN.CmsImpExp.csproj</b><br/><small>net9.0</small>"]
        click P7 "../projects/NiN.CmsImpExp.md"
    end
    P5 --> MAIN
    MAIN --> P7

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 424 |  |
| ***Total APIs Analyzed*** | ***424*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |
| xunit | 2.9.3 | — | 🔵 Optional | NuGet package is deprecated |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

