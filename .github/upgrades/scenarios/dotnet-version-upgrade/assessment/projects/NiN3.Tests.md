# NiN3.Tests\NiN3.Tests.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 3
- **Dependants**: 0
- **Number of Files**: 13
- **Number of Files with Incidents**: 2
- **Lines of Code**: 1437
- **Estimated LOC to modify**: 1+ (at least 0,1% of the project)

## Related Projects

**Depends on (3)** — projects this one references:

- [C:\Repo\Artsdatabanken\nin-kode-api\Infrastructure.Mapping\NiN3.Infrastructure.Mapping.csproj](../projects/NiN3.Infrastructure.Mapping.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Core\NiN3.Core.csproj](../projects/NiN3.Core.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Infrastructure\NiN3.Infrastructure.csproj](../projects/NiN3.Infrastructure.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["NiN3.Tests.csproj"]
        MAIN["<b>📦&nbsp;NiN3.Tests.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NiN3.Tests.md"
    end
    subgraph downstream["Dependencies (3)"]
        P10["<b>📦&nbsp;NiN3.Core.csproj</b><br/><small>net9.0</small>"]
        P11["<b>📦&nbsp;NiN3.Infrastructure.csproj</b><br/><small>net9.0</small>"]
        P12["<b>📦&nbsp;NiN3.Infrastructure.Mapping.csproj</b><br/><small>net9.0</small>"]
        click P10 "../projects/NiN3.Core.md"
        click P11 "../projects/NiN3.Infrastructure.md"
        click P12 "../projects/NiN3.Infrastructure.Mapping.md"
    end
    MAIN --> P10
    MAIN --> P11
    MAIN --> P12

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 1 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 2382 |  |
| ***Total APIs Analyzed*** | ***2383*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Microsoft.AspNetCore.Mvc.Testing | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.AspNetCore.TestHost | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.Extensions.Configuration | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |
| xunit | 2.9.3 | — | 🔵 Optional | NuGet package is deprecated |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

