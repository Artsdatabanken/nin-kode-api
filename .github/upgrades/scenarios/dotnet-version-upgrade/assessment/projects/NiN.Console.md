# NiN.Console\NiN.Console.csproj

[← Back to the assessment index](../../assessment.md)

## Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net11.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 4
- **Dependants**: 0
- **Number of Files**: 7
- **Number of Files with Incidents**: 2
- **Lines of Code**: 1781
- **Estimated LOC to modify**: 1+ (at least 0,1% of the project)

## Related Projects

**Depends on (4)** — projects this one references:

- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.Database\NiN.Database.csproj](../projects/NiN.Database.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.ExportImport\NiN.ExportImport.csproj](../projects/NiN.ExportImport.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NiN.Test_CmsImpExp\NiN.Test_CmsImpExp.csproj](../projects/NiN.Test_CmsImpExp.md)
- [C:\Repo\Artsdatabanken\nin-kode-api\NinKode.Database\NinKode.Database.csproj](../projects/NinKode.Database.md)

## Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["NiN.Console.csproj"]
        MAIN["<b>📦&nbsp;NiN.Console.csproj</b><br/><small>net9.0</small>"]
        click MAIN "../projects/NiN.Console.md"
    end
    subgraph downstream["Dependencies (4)"]
        P3["<b>📦&nbsp;NinKode.Database.csproj</b><br/><small>net9.0</small>"]
        P4["<b>📦&nbsp;NiN.Database.csproj</b><br/><small>net9.0</small>"]
        P8["<b>📦&nbsp;NiN.Test_CmsImpExp.csproj</b><br/><small>net9.0</small>"]
        P6["<b>📦&nbsp;NiN.ExportImport.csproj</b><br/><small>net9.0</small>"]
        click P3 "../projects/NinKode.Database.md"
        click P4 "../projects/NiN.Database.md"
        click P8 "../projects/NiN.Test_CmsImpExp.md"
        click P6 "../projects/NiN.ExportImport.md"
    end
    MAIN --> P3
    MAIN --> P4
    MAIN --> P8
    MAIN --> P6

```

## API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 1 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 2085 |  |
| ***Total APIs Analyzed*** | ***2086*** |  |

## NuGet Package Issues

| Package | Current Version | Suggested Version | Severity | Issue |
| :--- | :---: | :---: | :---: | :--- |
| AutoMapper | 14.0.0 | 17.0.0 | 🔵 Optional | NuGet package contains security vulnerability |
| Microsoft.EntityFrameworkCore.Abstractions | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.Extensions.Configuration.Json | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Microsoft.Extensions.Configuration.UserSecrets | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | 🟡 Potential | NuGet package upgrade is recommended |
| System.Text.Json | 9.0.4 | 10.0.15 | 🟡 Potential | NuGet package upgrade is recommended |

Every project affected by these packages, and the versions the repository settles on: [aggregate NuGet packages](../nuget/aggregate-packages.md).

