# NuGet package upgrade assessment

_Mode: **quick assessment** — package API diffs only; no per-project source scan was run._

## Recommended versions

- **coverlet.collector**: **10.1.1** (unified across 3 project(s)).
- **Microsoft.ApplicationInsights.AspNetCore**: **3.2.0** (unified across 1 project(s)).
- **Microsoft.AspNetCore.Mvc.Versioning**: **5.1.0** (unified across 1 project(s)).
- **Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer**: **5.1.0** (unified across 1 project(s)).
- **Microsoft.NET.Test.Sdk**: **18.12.0** (unified across 2 project(s)).
- **RavenDB.Client**: **7.2.8** (unified across 1 project(s)).
- **Swashbuckle.AspNetCore**: **11.0.1** (unified across 1 project(s)).
- **xunit**: **2.9.3** (unified across 2 project(s)).
- **xunit.runner.visualstudio**: **4.0.1** (unified across 13 project(s)).

## Public API changes

> **Types moved (namespace changed) are not removals.** A moved type keeps its name and members;
> the fix is a `using`-directive change, not a rewrite. Do not treat a moved type as deleted.

- **xunit.runner.visualstudio**: no source-breaking public API changes detected — see [`apidiff/xunit.runner.visualstudio.apidiff.md`](apidiff/xunit.runner.visualstudio.apidiff.md).
- **coverlet.collector**: no source-breaking public API changes detected — see [`apidiff/coverlet.collector.apidiff.md`](apidiff/coverlet.collector.apidiff.md).
- **Microsoft.NET.Test.Sdk**: no source-breaking public API changes detected — see [`apidiff/Microsoft.NET.Test.Sdk.apidiff.md`](apidiff/Microsoft.NET.Test.Sdk.apidiff.md).
- **RavenDB.Client**: 2 type(s) removed, 32 member(s) removed, 5 signature(s) changed — see [`apidiff/RavenDB.Client.apidiff.md`](apidiff/RavenDB.Client.apidiff.md).
- **Swashbuckle.AspNetCore**: no source-breaking public API changes detected — see [`apidiff/Swashbuckle.AspNetCore.apidiff.md`](apidiff/Swashbuckle.AspNetCore.apidiff.md).
- **Microsoft.ApplicationInsights.AspNetCore**: 22 type(s) removed, 39 member(s) removed — see [`apidiff/Microsoft.ApplicationInsights.AspNetCore.apidiff.md`](apidiff/Microsoft.ApplicationInsights.AspNetCore.apidiff.md).

## Breaking-change findings

- Version divergence findings (Pkg.0003): 0
- Requested-version-unsupported findings (Pkg.0002): 0

- Quick mode does not scan source, so there are no per-line `PkgApi` usage findings. Review the
  per-package API diffs above and rely on build errors during execution to pinpoint affected code.
- A full code scan can locate the exact source location of every breaking-change usage across the repo.
  It is opt-in and slower — re-run the assessment with `fullScan=true` only if the user requests it.

## Next steps

1. Proceed to planning to triage the API changes above and plan the code fixes (if any).

