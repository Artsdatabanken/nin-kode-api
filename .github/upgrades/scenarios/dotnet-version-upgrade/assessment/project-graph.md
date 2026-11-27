# Projects Relationship Graph

[← Back to the assessment index](../assessment.md)

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;NinKode.WebApi.csproj</b><br/><small>net9.0</small>"]
    P2["<b>📦&nbsp;NinKode.Common.csproj</b><br/><small>net9.0</small>"]
    P3["<b>📦&nbsp;NinKode.Database.csproj</b><br/><small>net9.0</small>"]
    P4["<b>📦&nbsp;NiN.Database.csproj</b><br/><small>net9.0</small>"]
    P5["<b>📦&nbsp;NiN.Console.csproj</b><br/><small>net9.0</small>"]
    P6["<b>📦&nbsp;NiN.ExportImport.csproj</b><br/><small>net9.0</small>"]
    P7["<b>📦&nbsp;NiN.CmsImpExp.csproj</b><br/><small>net9.0</small>"]
    P8["<b>📦&nbsp;NiN.Test_CmsImpExp.csproj</b><br/><small>net9.0</small>"]
    P9["<b>📦&nbsp;NiN3.Console.csproj</b><br/><small>net9.0</small>"]
    P10["<b>📦&nbsp;NiN3.Core.csproj</b><br/><small>net9.0</small>"]
    P11["<b>📦&nbsp;NiN3.Infrastructure.csproj</b><br/><small>net9.0</small>"]
    P12["<b>📦&nbsp;NiN3.Infrastructure.Mapping.csproj</b><br/><small>net9.0</small>"]
    P13["<b>📦&nbsp;NiN3.Tests.csproj</b><br/><small>net9.0</small>"]
    P1 --> P2
    P1 --> P3
    P1 --> P4
    P1 --> P11
    P2 --> P4
    P3 --> P2
    P3 --> P4
    P3 --> P6
    P5 --> P3
    P5 --> P4
    P5 --> P8
    P5 --> P6
    P6 --> P4
    P8 --> P7
    P9 --> P10
    P9 --> P11
    P11 --> P10
    P11 --> P12
    P12 --> P10
    P13 --> P10
    P13 --> P11
    P13 --> P12
    click P1 "projects/NinKode.WebApi.md"
    click P2 "projects/NinKode.Common.md"
    click P3 "projects/NinKode.Database.md"
    click P4 "projects/NiN.Database.md"
    click P5 "projects/NiN.Console.md"
    click P6 "projects/NiN.ExportImport.md"
    click P7 "projects/NiN.CmsImpExp.md"
    click P8 "projects/NiN.Test_CmsImpExp.md"
    click P9 "projects/NiN3.Console.md"
    click P10 "projects/NiN3.Core.md"
    click P11 "projects/NiN3.Infrastructure.md"
    click P12 "projects/NiN3.Infrastructure.Mapping.md"
    click P13 "projects/NiN3.Tests.md"

```

