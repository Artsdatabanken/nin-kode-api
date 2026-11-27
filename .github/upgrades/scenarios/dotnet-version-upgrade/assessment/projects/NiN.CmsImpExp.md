# NiN.CmsImpExp\NiN.CmsImpExp.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 7
- **Number of Files with Incidents**: 2
- **Lines of Code**: 800
- **Estimated LOC to modify**: 6+ (at least 0,8% of the project)

## Related Projects

**Depended on by (1)** — projects that reference this one:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.Test_CmsImpExp\NiN.Test_CmsImpExp.csproj](../projects/NiN.Test_CmsImpExp.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P8["<b>📦&nbsp;NiN.Test_CmsImpExp.csproj</b><br/><small>net9.0</small>"]
        click P8 "../projects/NiN.Test_CmsImpExp.md"
    end
    subgraph current["NiN.CmsImpExp.csproj"]
        MAIN["<b>📦&nbsp;NiN.CmsImpExp.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NiN.CmsImpExp.md"
    end
    P8 --> MAIN

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 1 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 5 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 799 |  |
| ***Total APIs Analyzed*** | ***805*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

