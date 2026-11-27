# NinKode.WebApi\NinKode.WebApi.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 4
- **Dependants**: 0
- **Number of Files**: 21
- **Number of Files with Incidents**: 9
- **Lines of Code**: 1010
- **Estimated LOC to modify**: 33+ (at least 3,3% of the project)

## Related Projects

**Depends on (4)** — projects this one references:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.Database\NiN.Database.csproj](../projects/NiN.Database.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN3.Infrastructure\NiN3.Infrastructure.csproj](../projects/NiN3.Infrastructure.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NinKode.Common\NinKode.Common.csproj](../projects/NinKode.Common.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NinKode.Database\NinKode.Database.csproj](../projects/NinKode.Database.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["NinKode.WebApi.csproj"]
        MAIN["<b>📦&nbsp;NinKode.WebApi.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NinKode.WebApi.md"
    end
    subgraph downstream["Dependencies (4)"]
        P2["<b>📦&nbsp;NinKode.Common.csproj</b><br/><small>net9.0</small>"]
        P3["<b>📦&nbsp;NinKode.Database.csproj</b><br/><small>net9.0</small>"]
        P4["<b>📦&nbsp;NiN.Database.csproj</b><br/><small>net9.0</small>"]
        P11["<b>📦&nbsp;NiN3.Infrastructure.csproj</b><br/><small>net9.0</small>"]
        click P2 "../projects/NinKode.Common.md"
        click P3 "../projects/NinKode.Database.md"
        click P4 "../projects/NiN.Database.md"
        click P11 "../projects/NiN3.Infrastructure.md"
    end
    MAIN --> P2
    MAIN --> P3
    MAIN --> P4
    MAIN --> P11

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 33 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1164 |  |
| ***Total APIs Analyzed*** | ***1197*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Microsoft.AspNetCore.Mvc.Versioning | 5.1.0 | — | 🔵 Optional | NuGet package is deprecated |
| Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer | 5.1.0 | — | 🔵 Optional | NuGet package is deprecated |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |
| System.Text.Encoding.CodePages | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

